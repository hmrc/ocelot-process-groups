using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.HttpSys;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductGrouping.Interfaces;
using ProductGrouping.Models;
using ProductGrouping.Repositories;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;
using System.Reflection;

namespace ProductGrouping
{
    /// <summary>
    /// Project startup class
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Startup contstructor
        /// </summary>
        /// <param name="configuration">Startup configuration</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// IConfiguration value
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Configure project services. This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">Services</param>
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

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "Product Grouping",
                    Description = "Grouping guidance products within the organisational structure",
                    TermsOfService = "to be confirmed",
                    Contact = new Contact
                    {
                        Name = "David Kinghan",
                        Email = "david.kinghan@hmrc.gov.uk",
                        Url = "to be confirmed"
                    },
                    License = new License
                    {
                        Name = "to be confirmed",
                        Url = "to be confirmed"
                    }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                c.IncludeXmlComments(xmlPath);
            });

            services.Configure<IISOptions>(c =>
            {
                c.ForwardClientCertificate = true;
                c.AutomaticAuthentication = true;
            });
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">Application Builder</param>
        /// <param name="env">Environment</param>
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
            app.UseSwagger();

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

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("../swagger/v1/swagger.json", "Product Grouping");
            });
        }
    }
}
