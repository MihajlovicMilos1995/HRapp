﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using HRApi.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;

namespace HRApi
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddAuthorization(options =>
            {
                
                options.AddPolicy("SuperUser",
                    authBuilder =>
                    {
                        authBuilder.RequireRole("SuperUser");
                    });

                options.AddPolicy("HrManager",
                   authBuilder =>
                   {
                       authBuilder.RequireRole("HrManager");
                   });

                options.AddPolicy("RegUser",
                   authBuilder =>
                   {
                       authBuilder.RequireRole("RegUser");
                   });

                options.AddPolicy("SuperUser, HrManager",
                   authBuilder =>
                   {
                       authBuilder.RequireRole("SuperUser", "HrManager");
                   });

                options.AddPolicy("SuperUser, HrManager,RegUser",
                   authBuilder =>
                   {
                       authBuilder.RequireRole("RegUser", "SuperUser", "HrManager");
                   });

                //options.GetPolicy("Admin");

                //options.GetPolicy("SubAdmin");

                //options.GetPolicy("RegUser");

            });

            var connectionString =
               @"Data Source=.\SQLEXPRESS;Initial Catalog=HRInfoDB;Integrated Security=True;MultipleActiveResultSets=True";

            services.AddDbContext<HRContext>
                (p => p.UseSqlServer(connectionString));

            services.AddIdentity<RegUser, IdentityRole>()
        .AddEntityFrameworkStores<HRContext>()
        .AddDefaultTokenProviders();

        //    using (var context = new HRContext())
        //    {
        //        context.Database.Migrate();
        //    }

            services.AddIdentity<RegUser, IdentityRole>(config =>
            {
                config.User.RequireUniqueEmail = true;
                config.Password.RequiredLength = 2;
                config.Password.RequireDigit = false;
                config.Password.RequireNonAlphanumeric = false;
                config.Password.RequireUppercase = false;
                config.Password.RequireLowercase = false;

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
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, RoleManager<IdentityRole> roleManager, UserManager<RegUser> userManager)
        {
            loggerFactory.AddConsole();

            app.UseIdentity();

            createRolesandUsers(roleManager, userManager);

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
                    defaults: new { controller = "Auth", action = "Login" });
            });
        }

        public async void createRolesandUsers(RoleManager<IdentityRole> roleManager, UserManager<RegUser> userManager)
        {
            if (!roleManager.RoleExistsAsync("SuperUser").Result)
            {

                // first we create Admin rool   
                var role = new IdentityRole();
                role.Name = "SuperUser";
                await roleManager.CreateAsync(role);

                //Here we create a Admin super user who will maintain the website                  

                var user = new RegUser();
                user.UserName = "milos";
                user.Email = "milos@hrapp.com";

                string userPWD = "sifra";

                var chkUser = await userManager.CreateAsync(user, userPWD);

                //Add default User to Role Admin   
                if (chkUser.Succeeded)
                {
                    var result1 = await userManager.AddToRoleAsync(user, "SuperUser");
                }
            }

            var res = await roleManager.RoleExistsAsync("HrManager");
            // creating Creating Manager role    
            if (res == false)
            {
                var role = new IdentityRole();
                role.Name = "HrManager";
                await roleManager.CreateAsync(role);
            }

            res = await roleManager.RoleExistsAsync("RegUser");
            // creating Creating Employee role    
            if (res == false)
            {
                var role = new IdentityRole();
                role.Name = "RegUser";
                await roleManager.CreateAsync(role);
            }
        }
    }
}
