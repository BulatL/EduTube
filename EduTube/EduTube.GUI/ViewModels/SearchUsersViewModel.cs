using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduTube.GUI.ViewModels
{
   public class SearchUsersViewModel
   {
      public string Id { get; set; }
      public string Firstname { get; set; }
      public string Lastname { get; set; }
      public string ChannelName { get; set; }
      public string ChannelDescription { get; set; }
      public string ProfileImage { get; set; }
      public DateTime DateOfBirth { get; set; }

      public SearchUsersViewModel()
      {
      }

      public SearchUsersViewModel(string id, string firstname, string lastname, string channelName,
          string channelDescription, string profileImage, DateTime dateOfBirth)
      {
         Id = id;
         Firstname = firstname;
         Lastname = lastname;
         ChannelName = channelName;
         ChannelDescription = channelDescription;
         ProfileImage = profileImage;
         DateOfBirth = dateOfBirth;
      }
   }
}
