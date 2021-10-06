using Endpoints.WebApi.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Endpoints.WebApi
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddHttpContextAccessor()
                .AddCustomizedCors(_configuration)
                .AddServiceRegistry()
                .ConfigApiVersioning()
                .AddCustomizeControllers()
                .ConfigMassTransit(_configuration)
                .ConfigApiBehavior()
                .AddCustomizedDataStore(_configuration)
                .AddCustomDataProtection()
                .ConfigMediatR()
                .AddCustomHealthCheck(_configuration)
                .ConfigTokenAuthentication(_configuration)
                .AddResponseCompression()
                .ConfigSwagger(_configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
                app.UseHttpsRedirection();
            }
            app.UseCustomizedCors(_configuration);
            app.UpdateDatabase();
            app.UseCustomizedSwagger(_configuration);
            app.UseCustomizedStaticFiles(env);
            app.CustomExceptionMiddleware();
            app.UseCustomizedRouting();
            app.UseCustomizedResponseCompression();
        }
    }
}