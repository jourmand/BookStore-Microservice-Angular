using BookStore.Endpoints.Oauth.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BookStore.Endpoints.Oauth
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
                .AddCustomizedCors(_configuration)
                .AddCustomizedDataStore(_configuration)
                .AddCustomizedIdentity()
                .ConfigIdentityServer(_configuration)
                .AddHealthChecks(_configuration)
                .AddCustomizeControllers()
                .AddResponseCompression()
                .ConfigMassTransit(_configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseStatusCodePagesWithRedirects("~/error/code/{0}");
                app.UseHsts();
                app.UseHttpsRedirection();
            }

            app.UseCustomizedCors(_configuration);
            app.UpdateDatabase();
            app.UseStaticFiles();
            app.UseIdentityServer();
            app.UseCustomizedStaticFiles(env);
            app.UseCustomizedRouting();
            app.UseCustomizedResponseCompression();
        }
    }
}
