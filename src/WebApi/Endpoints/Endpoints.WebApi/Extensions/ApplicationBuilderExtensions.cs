using System;
using Endpoints.WebApi.Extensions.Middleware;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Net.Http.Headers;
using WebApi.Infrastructures.Data.Commons;

namespace Endpoints.WebApi.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static void UseCustomizedResponseCompression(this IApplicationBuilder app) => app.UseResponseCompression();

        public static void UseCustomizedRouting(this IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/hc", new HealthCheckOptions
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

        public static void CustomExceptionMiddleware(this IApplicationBuilder app) =>
            app.UseMiddleware<ExceptionMiddleware>();

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

        public static IApplicationBuilder UseCustomizedSwagger(this IApplicationBuilder app, IConfiguration configuration)
        {
            if (configuration.GetValue<bool>("SwaggerEnabled"))
                app.UseSwagger()
                    .UseSwaggerUI(c =>
                    {
                        c.SwaggerEndpoint("/swagger/v1/swagger.json", "BookStore V1");
                    });

            return app;
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

            using var logContext = serviceScope.ServiceProvider.GetService<BookStoreDbContext>();
            logContext?.Database.Migrate();
        }
    }
}