using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using EduTube.DAL.Data;
using Microsoft.AspNetCore.Identity;
using EduTube.DAL.Entities;
using EduTube.BLL.Extensions;
using EduTube.GUI.Services.Interface;
using EduTube.GUI.Services;
using Microsoft.AspNetCore.Http.Features;
using EduTube.GUI.Validators;
using Microsoft.AspNetCore.Authentication.Cookies;
using EduTube.GUI.SignalRHubs;

namespace EduTube.GUI
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
         services.Configure<CookiePolicyOptions>(options =>
         {
            // This lambda determines whether user consent for non-essential cookies is needed for a given request.
            options.CheckConsentNeeded = context => true;
            options.MinimumSameSitePolicy = SameSiteMode.None;
         });


         services.AddDbContext<ApplicationDbContext>(options =>
              options.UseSqlServer(
                   Configuration.GetConnectionString("DefaultConnection")));

         services.AddScoped<IPasswordValidator<ApplicationUser>, MyPasswordValidator>();

         services.AddIdentity<ApplicationUser, IdentityRole>(
             options =>
             {
                options.Stores.MaxLengthForKeys = 128;
                options.User.RequireUniqueEmail = true;
             })
             .AddEntityFrameworkStores<ApplicationDbContext>()
             .AddDefaultUI()
             .AddDefaultTokenProviders();

         services.Configure<IdentityOptions>(options =>
         {
            // Password settings.
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = true;
            options.Password.RequiredLength = 6;
            options.Password.RequiredUniqueChars = 1;
            // user settings.
            //options.User.RequireUniqueEmail = true;
            //options.SignIn.RequireConfirmedEmail = true;
         });

         services.RegisterBLLServices();
         services.AddScoped<IUploadService, UploadService>();
         services.AddSignalR();
         services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

			services.PostConfigure<CookieAuthenticationOptions>(IdentityConstants.ApplicationScheme,
			 opt => {
				 //configure your other properties
				 opt.LoginPath = "/Login";
			 });
			/*services.Configure<FormOptions>(x =>
         {
             x.ValueLengthLimit = int.MaxValue;
             x.MultipartBodyLengthLimit = int.MaxValue; // In case of multipart
         });*/
		}

      // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
      public void Configure(IApplicationBuilder app, IHostingEnvironment env)
      {
         if (env.IsDevelopment())
         {
            app.UseDeveloperExceptionPage();
         }
         else
         {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
         }

         app.UseHttpsRedirection();
         app.UseStaticFiles();
         app.UseCookiePolicy();
         app.UseAuthentication();
         app.UseSignalR(route =>
         {
            route.MapHub<ChatHub>("/Chats/Index");
         });
         app.UseMvc(routes =>
         {
            routes.MapRoute(
                   name: "areas",
                   template: "{area:exists}/{controller=Admin}/{action=Index}/{id?}");

            routes.MapRoute(
                        name: "default",
                        template: "{controller=Home}/{action=Index}/{id?}");
         });
      }
   }
}
