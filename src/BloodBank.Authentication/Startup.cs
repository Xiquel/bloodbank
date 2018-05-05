using System;
using System.Collections.Generic;
using System.Linq;
using Npgsql.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Swagger;
using BloodBank.Core.Infrastructure;
using BloodBank.Authentication.Framework;
using BloodBank.Authentication.Models;
using Dotnet.Microservice.Netcore;

namespace BloodBank.Authentication
{
    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddIdentity<User, IdentityRole>(options => {
               
            })
            .AddEntityFrameworkStores<AuthenticationContext>()
            .AddDefaultTokenProviders().AddPasswordlessLoginTotpTokenProvider();
            services.AddDbContextPool<AuthenticationContext>(options => {
                options.UseNpgsql(configuration.GetConnectionString("Auth"), o => o.MigrationsHistoryTable("migration_history"));
            }).AddEntityFrameworkNpgsql();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "Authentication API",
                    Description = "Authentication API for the bloodbank project",
                    TermsOfService = "None",
                    Contact = new Contact
                    {
                        Name = "Sifiso Shezi",
                        Email = "sfiso.dlaba@gmail.com",
                        Url = "https://twitter.com/sphiecoh"
                    }
                });
            });
            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                // If the LoginPath isn't set, ASP.NET Core defaults 
                // the path to /Account/Login.
                options.LoginPath = "/Account/Login";
                // If the AccessDeniedPath isn't set, ASP.NET Core defaults 
                // the path to /Account/AccessDenied.
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.SlidingExpiration = true;
            });
            services.AddSingleton<IBus,RabbitBus>();
            var settings = new RabbitMqSettings();
            configuration.Bind("RabbitMq", settings);
            services.AddRabbitMq(settings);
            HealthChecker.AddHealthChecks(configuration);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            InitializeDatabase(app);
            if (env.IsDevelopment())
            {
      
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;
            });
            app.UseHealthEndpoint();
            app.UseMvc();
        }
        private void InitializeDatabase(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                scope.ServiceProvider.GetRequiredService<AuthenticationContext>().Database.Migrate(); 
            }
        }
    }
}
