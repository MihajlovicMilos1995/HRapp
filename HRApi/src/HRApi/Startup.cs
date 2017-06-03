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
using Logon.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

namespace HRApi
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; set; }
        private IConfigurationBuilder builder;

        public Startup(IHostingEnvironment env)
        {
            builder = new ConfigurationBuilder()
         .SetBasePath(env.ContentRootPath)
         .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
         .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);
         //.AddJsonFile("secrets.json", optional: true, reloadOnChange: true);

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets("152227768162-tb269i7kukg7d2aa3nqn2a3k56fi4qle.apps.googleusercontent.com");
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddMvc(options =>
            {
                options.SslPort = 44321;
                options.Filters.Add(new RequireHttpsAttribute());
            });

            services.AddAutoMapper(typeof(Startup));

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

            //var connectionString =            
            //     @"Data Source = (localdb)\MSSQLLocalDB;Initial Catalog =HrInfoDB;Integrated Security=True;MultipleActiveResultSets=True;";

            services.AddDbContext<HRContext>
                (/*p => p.UseSqlServer(connectionString)*/
            options =>
            options.UseSqlServer(Configuration["Data:DefaultConnection:ConnectionString"]));

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
            app.UseStaticFiles();
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler();
            }

            app.UseIdentity();

            app.UseGoogleAuthentication(new GoogleOptions()
            {
                ClientId = "152227768162-tb269i7kukg7d2aa3nqn2a3k56fi4qle.apps.googleusercontent.com",
                ClientSecret = "XgD_M898RYv8ZYnRQ4IxnBel"
            });

            app.UseFacebookAuthentication(new FacebookOptions()
            {
                AppId = "1905353013066180",
                AppSecret = "341cda9858d36027e96ba7023252c71f"
            });

            createRolesandUsers(roleManager, userManager);
         

            app.UseMvc(config =>
            {
                config.MapRoute(
                    name: "Default",
                    template: "{controller}/{action}/{id?}",
                    defaults: new { controller = "Job", action = "Index" });
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
                user.UserName = "admin";
                user.Email = "admin@hrapp.com";
                user.EmailConfirmed = true;
                user.TwoFactorEnabled = false;

                string userPWD = "admin";

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
