using System;
using System.Linq;
using HealthChecks.UI.Client;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Net.Http.Headers;
using Oauth.Infrastructures.Data.DbContext;
using Oauth.Infrastructures.Data.Entities;

namespace BookStore.Endpoints.Oauth.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static void UseCustomizedResponseCompression(this IApplicationBuilder app) =>
            app.UseResponseCompression();
        
        public static void UseCustomizedRouting(this IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/hc", new HealthCheckOptions()
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
                endpoints.MapHealthChecks("/liveness", new HealthCheckOptions
                {
                    Predicate = r => r.Name.Contains("self")
                });
            });
        }

        public static void UseCustomizedStaticFiles(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseStaticFiles(new StaticFileOptions
                {
                    OnPrepareResponse = context =>
                    {
                        var headers = context.Context.Response.GetTypedHeaders();
                        headers.CacheControl = new CacheControlHeaderValue
                        {
                            NoCache = true,
                            NoStore = true,
                            MaxAge = TimeSpan.FromDays(-1)
                        };
                    }
                });
            }
            else
            {
                app.UseStaticFiles(new StaticFileOptions
                {
                    OnPrepareResponse = context =>
                    {
                        var headers = context.Context.Response.GetTypedHeaders();
                        headers.CacheControl = new CacheControlHeaderValue
                        {
                            Public = true,
                            MaxAge = TimeSpan.FromDays(60)
                        };
                    }
                });
            }
        }

        public static IApplicationBuilder UseCustomizedCors(this IApplicationBuilder app, IConfiguration configuration)
        {
            if (configuration.GetValue<bool>("CorsEnabled"))
                app.UseCors("Cors");

            return app;
        }

        public static void UpdateDatabase(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope();
            using var persistedGrantDbContext = serviceScope.ServiceProvider.GetService<PersistedGrantDbContext>();
            persistedGrantDbContext.Database.Migrate();
            using var configurationDbContext = serviceScope.ServiceProvider.GetService<ConfigurationDbContext>();
            configurationDbContext.Database.Migrate();
            using var identityDbContext = serviceScope.ServiceProvider.GetService<AppIdentityDbContext>();
            identityDbContext.Database.Migrate();

            app.InitializeDatabase();
        }

        private static void InitializeDatabase(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>()?.CreateScope();

            var configurationDbContext = serviceScope?.ServiceProvider.GetRequiredService<ConfigurationDbContext>();

            var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var userInfo = userManager.FindByEmailAsync("admin@live.com").GetAwaiter().GetResult();
            if (userInfo == null)
            {
                var identityResult = userManager.CreateAsync(
                    new ApplicationUser { Email = "admin@live.com", UserName = "admin@live.com" },
                    "admin@123").GetAwaiter().GetResult();
            }

            if (!configurationDbContext.Clients.Any())
            {
                foreach (var client in IdentityServerDataProvider.GetClients())
                {
                    configurationDbContext.Clients.Add(client.ToEntity());
                }
                configurationDbContext.SaveChanges();
            }

            if (!configurationDbContext.ApiScopes.Any())
            {
                foreach (var resource in IdentityServerDataProvider.GetApiScopes())
                {
                    configurationDbContext.ApiScopes.Add(resource.ToEntity());
                }
                configurationDbContext.SaveChanges();
            }

            if (!configurationDbContext.ApiResources.Any())
            {
                foreach (var resource in IdentityServerDataProvider.GetApiResources())
                {
                    configurationDbContext.ApiResources.Add(resource.ToEntity());
                }
                configurationDbContext.SaveChanges();
            }
        }
    }
}