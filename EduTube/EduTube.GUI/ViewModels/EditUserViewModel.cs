﻿using EduTube.BLL.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EduTube.GUI.ViewModels
{
   public class EditUserViewModel
   {
      public string Id { get; set; }
      [Required, Display(Name = "First name")]
      public string Firstname { get; set; }

      [Required, Display(Name = "Last name")]
      public string Lastname { get; set; }

      [Required, EmailAddress]
      public string Email { get; set; }

      [Required, Display(Name = "Channel name")]
      public string ChannelName { get; set; }

      [Required, Display(Name = "Channel description")]
      public string ChannelDescription { get; set; }

      public IFormFile ProfileImage { get; set; }

      [Required, Display(Name = "Date of birth")]
      public DateTime DateOfBirth { get; set; }
      public string OldImage { get; set; }

      public EditUserViewModel()
      {
      }

      public EditUserViewModel(ApplicationUserModel model)
      {
         Id = model.Id;
         Firstname = model.Firstname;
         Lastname = model.Lastname;
         Email = model.Email;
         ChannelName = model.ChannelName;
         ChannelDescription = model.ChannelDescription;
         OldImage = model.ProfileImage;
         DateOfBirth = model.DateOfBirth;
      }

      public static ApplicationUserModel CopyToModel(EditUserViewModel viewModel)
      {
         return new ApplicationUserModel()
         {
            Id = viewModel.Id,
            Firstname = viewModel.Firstname,
            Lastname = viewModel.Lastname,
            Email = viewModel.Email,
            ChannelName = viewModel.ChannelName,
            ChannelDescription = viewModel.ChannelDescription,
            ProfileImage = viewModel.OldImage,
            DateOfBirth = viewModel.DateOfBirth
         };
      }
   }
}
