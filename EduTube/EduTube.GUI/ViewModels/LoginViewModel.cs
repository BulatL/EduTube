using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EduTube.GUI.ViewModels
{
   public class LoginViewModel
   {
      [Required]
      public string Password { get; set; }
      [Required, EmailAddress]
      public string Email { get; set; }
      public bool RememberMe { get; set; }
      public string RedirectUrl { get; set; }
   }
}
