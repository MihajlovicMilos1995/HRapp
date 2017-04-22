using System.Threading.Tasks;
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

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            ConfigureAuth(app);

            createRolesandUsers();

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
                    defaults: new { controller = "job", action = "getjob" });
            });
        }

        public void createRolesandUsers()
        {
            HRContext context = new HRContext(options);

            var roleManager = new RoleManager<IdentityRole>
                (new RoleStore<IdentityRole>(context));

            var UserManager = new UserManager<IdentityUser>
                (new UserStore<IdentityUser>(context));


            if (!roleManager.RoleExistsAsync("SuperUser").Result)
            {

                // first we create Admin rool   
                var role = new IdentityRole();
                role.Name = "SuperUser";
                roleManager.CreateAsync(role);

                //Here we create a Admin super user who will maintain the website                  

                var user = new IdentityUser();
                user.UserName = "milos";
                user.Email = "milos@hrapp.com";

                string userPWD = "sifra";

                var chkUser = UserManager.CreateAsync(user, userPWD).Result;

                //Add default User to Role Admin   
                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRoleAsync(user, "SuperUser").Result;

                }
            }

            // creating Creating Manager role    
            if (roleManager.RoleExistsAsync("Manager").Result == false)
            {
                var role = new IdentityRole();
                role.Name = "HrManager";
                roleManager.CreateAsync(role);

            }

            // creating Creating Employee role    
            if (roleManager.RoleExistsAsync("RegUser").Result == false)
            {
                var role = new IdentityRole();
                role.Name = "RegUser";
                roleManager.CreateAsync(role);

            }
        }
    }
}
