using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using HRApi.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace HRApi
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            var connectionString =
               @"Data Source=.\SQLEXPRESS;Initial Catalog=HRInfoDB3;Integrated Security=True;MultipleActiveResultSets=True";

            services.AddDbContext<HRContext>
                (p => p.UseSqlServer(connectionString));

            services.AddIdentity<IdentityUser, IdentityRole>()
        .AddEntityFrameworkStores<HRContext>()
        .AddDefaultTokenProviders();

            services.AddIdentity<IdentityUser, IdentityRole>(config =>
            {
                config.User.RequireUniqueEmail = true;
                config.Password.RequiredLength = 6;
                config.Cookies.ApplicationCookie.LoginPath = "/Auth/Login";
                config.Cookies.ApplicationCookie.Events = new CookieAuthenticationEvents()
                {
                    OnRedirectToLogin = async ctx =>
                    {
                        if (ctx.Request.Path.StartsWithSegments("/Job") &&
                        ctx.Response.StatusCode == 200)
                        {
                            ctx.Response.StatusCode = 401;
                        }
                        else
                        {
                            ctx.Response.Redirect(ctx.RedirectUri);
                        }
                        await Task.Yield();
                    }
                };
            })
           .AddEntityFrameworkStores<HRContext>();

            //         services.Configure<IdentityOptions>(options =>
            //         {
            //             // Password settings
            //             options.Password.RequireDigit = false;
            //             options.Password.RequiredLength = 2;
            //             options.Password.RequireNonAlphanumeric = false;
            //             options.Password.RequireUppercase = false;
            //             options.Password.RequireLowercase = false;
            //
            //             // Lockout settings
            //             options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
            //             options.Lockout.MaxFailedAccessAttempts = 10;
            //
            //             // Cookie settings
            //             options.Cookies.ApplicationCookie.ExpireTimeSpan = TimeSpan.FromDays(150);
            //             options.Cookies.ApplicationCookie.LoginPath = "/Auth/Login";
            //
            //             // User settings
            //             options.User.RequireUniqueEmail = true;
            //         });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler();
            }

            app.UseMvc(config =>
            {
                config.MapRoute(
                    name: "Default",
                    template: "{controller}/{action}/{id?}",
                    defaults: new { controller = "user", action = "getuser" });
            });
        }
    }
}
