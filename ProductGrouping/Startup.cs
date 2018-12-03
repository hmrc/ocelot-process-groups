 using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.HttpSys;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductGrouping.Interfaces;
using ProductGrouping.Models;
using ProductGrouping.Repositories;
using System;

namespace ProductGrouping
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<Context>(options =>
                options
                    .UseSqlServer(Configuration.GetConnectionString("DefaultConnection") +
                            Environment.GetEnvironmentVariable("Connection", EnvironmentVariableTarget.Machine)));

            services.AddAuthentication(HttpSysDefaults.AuthenticationScheme);

            services.AddMvc();

            services.AddScoped<IProductGroupRepository, ProductGroupRepository>();
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<ILegacyFileRepository, LegacyFileRepository>();

            services.Configure<IISOptions>(c =>
            {
                c.ForwardClientCertificate = true;
                c.AutomaticAuthentication = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Shared/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<Context>();

                context.Database.EnsureCreated();
            }

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });
        }
    }
}
