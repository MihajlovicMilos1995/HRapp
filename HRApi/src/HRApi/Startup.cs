using HRApi.Models;
using Logon.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace HRApi
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddMvc(options =>
            {
                options.SslPort = 44321;
                //pitaj mentore
                //options.Filters.Add(new RequestHttpsAttribute());
            });

            services.AddTransient<IEmailSender, AuthMessageSender>();

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
            //smart db
            // var connectionString =
            //    @"Data Source=.\SQLEXPRESS;Initial Catalog=HRInfoDB;Integrated Security=True;MultipleActiveResultSets=True";

            var connectionString =
                @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=HrInfoDB;Integrated Security = True;MultipleActiveResultSets=true";

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

                // TODO: 2) Confirmed mail
                //
                // Another example of what you can do is to require a confirmed email:
                //
                //services.Configure<IdentityOptions>(options => {
                //    options.SignIn.RequireConfirmedEmail = true;
                //});
                //
                // If you do this you won’t have to check the EmailConfirmed property of
                // IdentityUser.When you try to sign the user in using the SignInManager
                // it will fail, and the result will contain a property named IsNotAllowed
                // set to true.

                config.SignIn.RequireConfirmedEmail = true;
                config.Password.RequiredLength = 2;
                config.Password.RequireDigit = false;
                config.Password.RequireNonAlphanumeric = false;
                config.Password.RequireUppercase = false;
                config.Password.RequireLowercase = false;

                config.Cookies.ApplicationCookie.LoginPath = "/Account/Login";
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
            var builder = new ConfigurationBuilder()
                    .SetBasePath(env.ContentRootPath)
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);


            loggerFactory.AddConsole();

            app.UseIdentity();

            CreateRolesandUsers(roleManager, userManager);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //builder.AddUserSecrets<Startup>();
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
                    defaults: new { controller = "Account", action = "Login" });
            });
        }

        public async void CreateRolesandUsers(RoleManager<IdentityRole> roleManager, UserManager<RegUser> userManager)
        {
            if (!roleManager.RoleExistsAsync("SuperUser").Result)
            {

                // first we create Admin rool   
                var role = new IdentityRole()
                {
                    Name = "SuperUser"
                };
                await roleManager.CreateAsync(role);

                //Here we create a Admin super user who will maintain the website                  

                var user = new RegUser()
                {
                    UserName = "milos",
                    Email = "milos@hrapp.com"
                };

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
                var role = new IdentityRole()
                {
                    Name = "HrManager"
                };
                await roleManager.CreateAsync(role);
            }

            res = await roleManager.RoleExistsAsync("RegUser");
            // creating Creating Employee role    
            if (res == false)
            {
                var role = new IdentityRole()
                {
                    Name = "RegUser"
                };
                await roleManager.CreateAsync(role);
            }
        }
    }
}
