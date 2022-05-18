using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TaskList.Models;

namespace TaskList
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

            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddRazorPages();

            //Settings for Connection to Database.
            //Connection string is in appsettings.json file.
            string connString = "ConnectionStrings:TaskListConnection";
            services.AddDbContext<TaskListDbContext>(opts => opts.UseSqlServer(Configuration[connString]));

            //Identity Service.
            string identityConnString = "ConnectionStrings:IdentityConnection";
            services.AddDbContext<IdentityContext>
                (opts => opts.UseSqlServer
                                    (Configuration[identityConnString]));

            services.AddIdentity<IdentityUser, IdentityRole>()
                                        .AddEntityFrameworkStores<IdentityContext>();

            //Adding services for working with Data.(Dependency Injection)
            services.AddScoped<IAssignmentRepository, EFAssignmentRepository>();
            services.AddScoped<IUsersRepository, EFUserRepository>();

            services.Configure<IdentityOptions>(opts =>
            {
                opts.User.RequireUniqueEmail = true;
            });


            //Redirection Path , when user need to pass Authentication
            //Or Authorization.

            services.Configure<CookieAuthenticationOptions>(IdentityConstants.ApplicationScheme,
                opts => {
                    opts.LoginPath = "/Users/Login";
                    opts.AccessDeniedPath = "/Users/AccessDenied";
                });


            //In Get methods , json with null value will be ignored.
            services.Configure<JsonOptions>(opts =>
            {
                opts.JsonSerializerOptions.IgnoreNullValues = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env ,
                              TaskListDbContext context,IdentityContext identCtx)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {

                endpoints.MapDefaultControllerRoute();
                endpoints.MapRazorPages();

            });

            //Creation of DB if it`s doesn`t exists.
            //And minimal Data For Db.
            SeedData.Seed(context);
            //Creation of Identity Database if it`s doesn`t exists.
            //Admin inicialization.
            IdentitySeedData.CreateAdminAccount(app.ApplicationServices, Configuration, identCtx);
        }
    }
}
