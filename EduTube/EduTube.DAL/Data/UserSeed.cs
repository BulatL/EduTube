using EduTube.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EduTube.DAL.Data
{
   public class UserSeed
   {
      public static async Task SeedAsync(ApplicationDbContext context,
          UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
      {
         context.Database.EnsureCreated();

         string role1 = "Admin";
         string role2 = "User";

         if (await roleManager.FindByNameAsync(role1) == null)
            await roleManager.CreateAsync(new IdentityRole(role1));

         if (await roleManager.FindByNameAsync(role2) == null)
            await roleManager.CreateAsync(new IdentityRole(role2));

         if (await userManager.FindByNameAsync("Admin1") == null)
         {
            ApplicationUser user = new ApplicationUser()
            {
               UserName = "Admin1@gmail.com",
               Email = "Admin1@gmail.com",
               ChannelName = "Admin1",
               Firstname = "Admin",
               Lastname = "Admin",
               ChannelDescription = "Admin",
               ProfileImage = "Image1",
               DateOfBirth = DateTime.Today,
               Blocked = false,
               Deleted = false
            };
            IdentityResult result = await userManager.CreateAsync(user);
            if (result.Succeeded)
            {
               await userManager.AddPasswordAsync(user, "Sifra123");
               await userManager.AddToRoleAsync(user, role1);
            }
         }

         if (await userManager.FindByNameAsync("User1") == null)
         {
            ApplicationUser user = new ApplicationUser()
            {
               UserName = "User1@gmail.com",
               Email = "User1@gmail.com",
               ChannelName = "User1",
               Firstname = "User",
               Lastname = "User",
               ChannelDescription = "User",
               ProfileImage = "Image1",
               DateOfBirth = DateTime.Today,
               Blocked = false,
               Deleted = false
            };
            IdentityResult result = await userManager.CreateAsync(user);
            if (result.Succeeded)
            {
               await userManager.AddPasswordAsync(user, "Sifra123");
               await userManager.AddToRoleAsync(user, role2);
            }
         }

         if (await userManager.FindByNameAsync("User2") == null)
         {
            ApplicationUser user = new ApplicationUser()
            {
               UserName = "User2@gmail.com",
               Email = "User2@gmail.com",
               ChannelName = "User2",
               Firstname = "User",
               Lastname = "User",
               ChannelDescription = "User",
               ProfileImage = "Image1",
               DateOfBirth = DateTime.Today,
               Blocked = false,
               Deleted = false
            };
            IdentityResult result = await userManager.CreateAsync(user);
            if (result.Succeeded)
            {
               await userManager.AddPasswordAsync(user, "Sifra123");
               await userManager.AddToRoleAsync(user, role2);
            }
         }
      }
   }
}
