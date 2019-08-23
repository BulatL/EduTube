﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EduTube.DAL.Data;
using EduTube.DAL.Entities;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EduTube.GUI
{
   public class Program
   {
      public static void Main(string[] args)
      {
         //CreateWebHostBuilder(args).Build().Run();
         var host = CreateWebHostBuilder(args).Build();

         using (var scope = host.Services.CreateScope())
         {
            var services = scope.ServiceProvider;
            var appContext = services.GetRequiredService<ApplicationDbContext>();

            try
            {
               var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
               var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

               UserSeed.SeedAsync(appContext, userManager, roleManager).Wait();
            }
            catch (Exception ex)
            {
               Console.WriteLine(ex.StackTrace);
            }
         }

         host.Run();
      }

      public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
          WebHost.CreateDefaultBuilder(args)
              .UseStartup<Startup>();
   }
}
