using EduTube.BLL.Models;
using EduTube.GUI.Validators;
using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace EduTube.GUI.ViewModels
{
   public class RegisterViewModel
   {
      [Required, Display(Name ="First name")]
      public string Firstname { get; set; }

      [Required, Display(Name = "Last name")]
      public string Lastname { get; set; }

      [Required, EmailAddress]
      public string Email { get; set; }

      [Required, Display(Name = "Channel name")]
      public string ChannelName { get; set; }

      [Required, Display(Name = "Channel description")]
      public string ChannelDescription { get; set; }

      [Required]
      [DataType(DataType.Password)]
      [MyPasswordValidator]
      public string Password { get; set; }

      [Required]
      [DataType(DataType.Password)]
      [Display(Name = "Confirm Password")]
      [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
      public string ConfirmPassword { get; set; }

      public IFormFile ProfileImage { get; set; }

      [Required, Display(Name = "Date of birth")]
      public DateTime DateOfBirth { get; set; }

      public RegisterViewModel()
      {
      }

      public RegisterViewModel(ApplicationUserModel model)
      {
         Firstname = model.Firstname;
         Lastname = model.Lastname;
         Email = model.Email;
         ChannelName = model.ChannelName;
         ChannelDescription = model.ChannelDescription;
         //OldImage = model.ProfileImage;
         DateOfBirth = model.DateOfBirth;
      }

      public static ApplicationUserModel CopyToModel(RegisterViewModel viewModel)
      {
         return  new ApplicationUserModel()
         {
            //Id = viewModel.Id,
            Firstname = viewModel.Firstname,
            Lastname = viewModel.Lastname,
            Email = viewModel.Email,
            ChannelName = viewModel.ChannelName,
            ChannelDescription = viewModel.ChannelDescription,
            //ProfileImage = viewModel.OldImage,
            DateOfBirth = viewModel.DateOfBirth
         };
      }
   }
}
