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
      [Required]
      public string Firstname { get; set; }
      [Required]
      public string Lastname { get; set; }
      [Required]
      public string ChannelName { get; set; }
      [Required]
      public string ChannelDescription { get; set; }
      public string Password { get; set; }
      public IFormFile ProfileImage { get; set; }
      [Required]
      public DateTime DateOfBirth { get; set; }
   }
}
