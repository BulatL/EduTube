using EduTube.GUI.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EduTube.GUI.ViewModels
{
   public class ChangePasswordViewModel
   {
      public string UserId { get; set; }

      [Required]
      [DataType(DataType.Password)]
      [Display(Name = "Old Password")]
      public string OldPassword { get; set; }

      [Required]
      [DataType(DataType.Password)]
      [Display(Name = "New Password")]
      [MyPasswordValidatorAttribute]
      public string NewPassword { get; set; }

      [Required]
      [DataType(DataType.Password)]
      [Display(Name = "Confirm Password")]
      [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
      public string ConfirmPassword { get; set; }

      public ChangePasswordViewModel()
      {
      }
      public ChangePasswordViewModel(string id)
      {
         UserId = id;
      }
   }
}
