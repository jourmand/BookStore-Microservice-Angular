using System;
using System.Reflection;
using System.Text.Json;
using GreenPipes;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Oauth.Infrastructures.Data.DbContext;
using Oauth.Infrastructures.Data.Entities;
using Oauth.Infrastructures.Data.IntegrationEvents.EventHandling;

namespace BookStore.Endpoints.Oauth.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHealthChecks()
                .AddCheck("self", () => HealthCheckResult.Healthy())
                .AddSqlServer(configuration.GetConnectionString("DatabaseConnection"),
                    name: "IdentityDB-check",
                    tags: new[] { "IdentityDB" });

            return services;
        }

        public static IServiceCollection AddCustomizeControllers(this IServiceCollection services)
        {
            services.AddControllers()
                .SetCompatibilityVersion(CompatibilityVersion.Latest)
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.IgnoreNullValues = true;
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                });
            return services;
        }

        public static IServiceCollection ConfigMassTransit(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumer<UserCreatedIntegrationEventHandler>();
                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    cfg.Host(new Uri($"rabbitmq://{configuration["EventBus:Connection"]}/"), h =>
                    {
                        h.Username(configuration["EventBus:UserName"]);
                        h.Password(configuration["EventBus:Password"]);
                    });

                    cfg.ReceiveEndpoint(configuration["EventBus:SubscriptionClientName"], ep =>
                    {
                        ep.PrefetchCount = 16;
                        ep.UseMessageRetry(r => r.Interval(2, 1000));
                        ep.ConfigureConsumer<UserCreatedIntegrationEventHandler>(provider);
                        ep.UseInMemoryOutbox();
                    });
                }));
            });

            services.AddMassTransitHostedService();
            return services;
        }

        public static IServiceCollection ConfigIdentityServer(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DatabaseConnection");
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            services.AddIdentityServer(x =>
            {
                x.IssuerUri = null;
                x.Endpoints.EnableUserInfoEndpoint = false;
                x.Endpoints.EnableEndSessionEndpoint = false;
            })
            .AddSigningCredential(Certificate.Certificate.Get())
            .AddAspNetIdentity<ApplicationUser>()
            .AddConfigurationStore(options =>
            {
                options.ConfigureDbContext = builder => builder.UseSqlServer(connectionString,
                    sqlOptions =>
                    {
                        sqlOptions.MigrationsAssembly(migrationsAssembly);
                        sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                    });
            })
            .AddOperationalStore(options =>
            {
                options.ConfigureDbContext = builder => builder.UseSqlServer(connectionString,
                    sqlOptions =>
                    {
                        sqlOptions.MigrationsAssembly(migrationsAssembly);
                        sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                    });
                options.EnableTokenCleanup = true;
                options.TokenCleanupInterval = 30;
            });
            return services;
        }

        public static IServiceCollection AddCustomizedDataStore(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddDbContext<AppIdentityDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("OauthConnection"),
                    b =>
                    {
                        b.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name);
                        b.MigrationsHistoryTable($"__{nameof(AppIdentityDbContext)}");
                    }));
            return services;
        }

        public static IServiceCollection AddCustomizedIdentity(this IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequiredLength = 6;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.User.RequireUniqueEmail = false;
                    options.Lockout.MaxFailedAccessAttempts = 3;
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
                })
                .AddEntityFrameworkStores<AppIdentityDbContext>()
                .AddDefaultTokenProviders();

            return services;
        }

        public static IServiceCollection AddCustomizedCors(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration.GetValue<bool>("CorsEnabled"))
                services.AddCors(options => options.AddPolicy("Cors", builder =>
                {
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                }));

            return services;
        }
    }
}
