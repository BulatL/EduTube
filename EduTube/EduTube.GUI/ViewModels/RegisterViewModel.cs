using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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
      public string Password { get; set; }
      public IFormFile ProfileImage { get; set; }
      [Required, Display(Name = "Date of birth")]
      public DateTime DateOfBirth { get; set; }
   }
}
