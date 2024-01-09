using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Route.BLL;
using Route.BLL.Interfaces;
using Route.BLL.Repositories;
using Route.DAL.Data;
using Route.DAL.Models;
using Route_MVC_App.PL.Helpers;
using Route_MVC_App.PL.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Route_MVC_App
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
            services.AddControllersWithViews();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
            services.AddAutoMapper(M=>M.AddProfile(new MappingProfile()));

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequiredUniqueChars = 2;
                options.Password.RequireDigit = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 5;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.User.RequireUniqueEmail = true;
                options.Lockout.MaxFailedAccessAttempts = 3;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
                options.Lockout.AllowedForNewUsers = true;
            }).AddEntityFrameworkStores<AppDbContext>() 
            .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(config => {
                config.LoginPath = "/Account/SignIn";
                config.AccessDeniedPath="/Home/Error";
                config.ExpireTimeSpan=TimeSpan.FromMinutes(10);

                });
            services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));

            services.AddTransient<IMailSettings, EmailSetting>();
            services.Configure<TwillioSettings>(Configuration.GetSection("Twillio"));
            services.AddTransient<ISmsService, SmsService>();

            //services.AddAuthentication(o =>
            //{
            //    o.DefaultAuthenticateScheme = GoogleDefaults.AuthenticationScheme;
            //    o.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
            //}).AddGoogle(o=>
            //{
            //    IConfiguration GoogleAuthSection = Configuration.GetSection("Authentication:Google");
            //    o.ClientId = GoogleAuthSection["ClientId"];
            //    o.ClientSecret = GoogleAuthSection["ClientSecret"];    

            //});

            //services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            //    .AddCookie( config =>
            //    {
            //        config.LoginPath = "/Account/SignIn";
            //        config.AccessDeniedPath = "/Home/Error";
            //    });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
			app.UseAuthentication();

			app.UseAuthorization();




			app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
