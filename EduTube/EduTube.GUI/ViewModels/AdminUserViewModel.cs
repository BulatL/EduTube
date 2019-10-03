using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduTube.GUI.ViewModels
{
   public class AdminUserViewModel
   {
      public string Id { get; set; }
      public string ChannelName { get; set; }
      public bool Blocked { get; set; }
      public string Role { get; set; }
      public string ProfileImage { get; set; }

      public AdminUserViewModel()
      {

      }
      public AdminUserViewModel(string id, string channelName, bool blocked, string role, string profileImage)
      {
         Id = id;
         ChannelName = channelName;
         Blocked = blocked;
         Role = role;
         ProfileImage = profileImage;
      }
   }
}
