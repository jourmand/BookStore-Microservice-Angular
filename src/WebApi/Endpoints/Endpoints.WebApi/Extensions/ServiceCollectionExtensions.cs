using System;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using BuildingBlocks.Framework.WebToolkit.Attributes;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using WebApi.Core.ApplicationService.Commands.ApplicationUserAggregate.CreateUser;
using WebApi.Core.ApplicationService.Commands.BookAggregate.CreateBookItem;
using WebApi.Core.Domain.ApplicationUserAggregate.Contracts;
using WebApi.Core.Domain.BookAggregate.Contracts;
using WebApi.Core.Domain.Commons;
using WebApi.Infrastructures.Data.Commons;
using WebApi.Infrastructures.Data.Repositories;
using DomainEventHandlingExecutor = WebApi.Core.Domain.Commons.DomainEventHandlingExecutor;

namespace Endpoints.WebApi.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServiceRegistry(this IServiceCollection services)
        {
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<IPublishEvent, RabbitMqPublishEvent>();
            services.AddScoped<IBookItemLookup, BookItemLookup>();
            services.AddScoped<IApplicationUserLookup, ApplicationUserLookup>();
           
            services.AddScoped<IDomainEventHandlingExecutor, DomainEventHandlingExecutor>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddSingleton<IAuthorizationPolicyProvider, AuthorizationPolicyProvider>();
            services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();

            services.Scan(s => s.FromAssemblies(Assembly.Load(typeof(BookStoreDbContext).GetTypeInfo().Assembly.GetName().Name),
                    Assembly.Load(typeof(CreateBookItemCommand).GetTypeInfo().Assembly.GetName().Name),
                    Assembly.Load(typeof(ICurrentUserService).GetTypeInfo().Assembly.GetName().Name),
                    Assembly.Load(typeof(UnitOfWork).GetTypeInfo().Assembly.GetName().Name))
                .AddClasses(c => c.Where(type => type.Name.EndsWith("Repository")))
                .AsImplementedInterfaces()
                .WithScopedLifetime());

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
                x.AddBus(_ => Bus.Factory.CreateUsingRabbitMq(cfg =>
                {

                    cfg.Host(new Uri($"rabbitmq://{configuration["EventBus:Connection"]}/"), h =>
                    {
                        h.Username(configuration["EventBus:UserName"]);
                        h.Password(configuration["EventBus:Password"]);
                    });
                }));
            });

            services.AddMassTransitHostedService();
            return services;
        }

        public static IServiceCollection ConfigMediatR(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.Load(typeof(CreateUserCommand).GetTypeInfo().Assembly.GetName().Name),
                Assembly.Load(typeof(BookStoreDbContext).GetTypeInfo().Assembly.GetName().Name));
            return services;
        }

        public static IServiceCollection ConfigApiBehavior(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {

                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Instance = context.HttpContext.Request.Path,
                        Status = StatusCodes.Status400BadRequest,
                        Title = "Please refer to the errors property for additional details."
                    };

                    return new BadRequestObjectResult(new BuildingBlocks.Framework.Entities.ErrorDetails(problemDetails.Status, problemDetails.Title, problemDetails.Instance,
                        context.ModelState.Values.SelectMany(x => x.Errors)
                            .Select(x => new BuildingBlocks.Framework.Entities.Error(x.ErrorMessage))
                        ))
                    {
                        ContentTypes = { "application/problem+json", "application/problem+xml" }
                    };
                };
            });
            return services;
        }

        public static IServiceCollection ConfigApiVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(o =>
            {
                o.ReportApiVersions = true;
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.DefaultApiVersion = new ApiVersion(1, 0);
            });
            return services;
        }

        public static IServiceCollection ConfigSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration.GetValue<bool>("SwaggerEnabled"))
                services.AddSwaggerGen(options =>
                {
                    options.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Title = "Book Store HTTP API",
                        Version = "v1.0",
                        Description = "The Book Store Service HTTP API",
                    });
                    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Scheme = "Bearer",
                        In = ParameterLocation.Header,
                        Description = "Please insert JWT with Bearer into field",
                        Name = "Authorization",
                        Type = SecuritySchemeType.OAuth2,
                        Flows = new OpenApiOAuthFlows
                        {
                            Password = new OpenApiOAuthFlow
                            {
                                AuthorizationUrl = new Uri($"{configuration["Jwt:Authority"]}connect/authorize"),
                                TokenUrl = new Uri($"{configuration["Jwt:Authority"]}connect/token"),
                            }
                        }
                    });
                    options.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            Array.Empty<string>()
                        }
                    });
                });

            return services;
        }

        public static IServiceCollection ConfigTokenAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = configuration["Jwt:Authority"];
                    options.ApiName = configuration["Jwt:ApiName"];
                    options.ApiSecret = configuration["Jwt:ApiSecret"];
                    options.RequireHttpsMetadata = false;
                });

            services.AddAuthorization();

            return services;
        }


        public static IServiceCollection AddCustomDataProtection(this IServiceCollection services)
        {
            services
                .AddDataProtection();
            return services;
        }

        public static IServiceCollection AddCustomizedDataStore(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddDbContext<BookStoreDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DatabaseConnection"),
                b => b.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name)));

            return services;
        }

        public static IServiceCollection AddCustomHealthCheck(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHealthChecks()
                .AddCheck("self", () => HealthCheckResult.Healthy())
                .AddUrlGroup(new Uri($"{configuration["Jwt:Authority"]}hc"), name: "identityapi-check", tags: new[] { "identityapi" })
                .AddSqlServer(
                    configuration.GetConnectionString("DatabaseConnection"),
                    name: "Database-check",
                    tags: new[] { "main" });

            return services;
        }
    }
}