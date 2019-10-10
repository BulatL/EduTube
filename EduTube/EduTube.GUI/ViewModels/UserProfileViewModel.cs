using EduTube.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduTube.GUI.ViewModels
{
   public class UserProfileViewModel
   {
      public string Id { get; set; }
      public string ChannelName { get; set; }
      public string Firstname { get; set; }
      public string Lastname { get; set; }
      public string ChannelDescription { get; set; }
      public string ProfileImage { get; set; }
      public bool Blocked { get; set; }
      public string Role { get; set; }
      public List<VideoModel> Videos { get; set; }
      public List<SubscriptionModel> Subscribers { get; set; }
      public List<SubscriptionModel> SubscribedOn { get; set; }

      public UserProfileViewModel()
      {
      }

      public UserProfileViewModel(ApplicationUserModel user)
      {
         Id = user.Id;
         ChannelName = user.ChannelName;
         ChannelDescription = user.ChannelDescription;
         Firstname = user.Firstname;
         Lastname = user.Lastname;
         ProfileImage = user.ProfileImage;
         Blocked = user.Blocked;
         Videos = user.Videos;
         Subscribers = user.Subscribers;
         SubscribedOn = user.SubscribedOn;
      }
   }
}
