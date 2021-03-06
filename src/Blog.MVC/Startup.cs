﻿using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.MVC
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json",
                             optional: false,
                             reloadOnChange: true)
                .AddEnvironmentVariables();

            // builder.AddUserSecrets<Startup>();
            // UserSecrets were removed to simplify demo setup.
            // In a real production system, UserSecrets should be used to store sensitive info.

            Configuration = builder.Build();
        }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // This method gets called by the runtime.
            // Use this method to add services to the container.

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for 
                // non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = new PathString("/Login/Index");
                options.AccessDeniedPath = new PathString("/AccessDenied/Index");
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireAdminRole", policy =>
                {
                    policy.AddAuthenticationSchemes(CookieAuthenticationDefaults.AuthenticationScheme);
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("BlogUserId");
                    policy.RequireRole(Blog.Core.KeyChain.BlogUser_Permission_Admin);
                });
            });

            services.AddSingleton<IConfiguration>(this.Configuration);
            Blog.MVC.Setup.BlogCoreSetup.AddBlogCoreSingletons(services);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // This method gets called by the runtime.
            // Use this method to configure the HTTP request pipeline.

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            //app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
