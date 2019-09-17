using EduTube.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduTube.GUI.Validators
{
   public class MyPasswordValidator : IPasswordValidator<ApplicationUser>
   {
      public Task<IdentityResult> ValidateAsync(UserManager<ApplicationUser> manager, ApplicationUser user, string password)
      {
         return Task.FromResult(Validate(user, password));
      }

      private IdentityResult Validate(ApplicationUser user, string password)
      {
         if (password.Length < 6)
         {
            return IdentityResult.Failed(new IdentityError
            {
               Code = "PasswordTooShort",
               Description = "Passwords must be at least six characters"
            });
         }

         if (password.Contains(user.ChannelName, System.StringComparison.OrdinalIgnoreCase))
         {
            return IdentityResult.Failed(new IdentityError
            {
               Code = "ChannelnameInPassword",
               Description = "Passwords cannot contain the channel name"
            });
         }

         if (!password.Any(char.IsUpper))
         {
            return IdentityResult.Failed(new IdentityError
            {
               Code = "UppercaseCharacterInPassword",
               Description = "Passwords must contain at least 1 uppercase character"
            });
         }

         if (!password.Any(char.IsLower))
         {
            return IdentityResult.Failed(new IdentityError
            {
               Code = "NonUppercaseCharacterInPassword",
               Description = "Passwords must contain at least 1 non uppercase character"
            });
         }

         return IdentityResult.Success;
      }
   }
}
