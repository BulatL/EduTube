using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduTube.BLL.Models
{
   public class ApplicationUserModel : IdentityUser
   {
      public string ChannelName { get; set; }
      public string Firstname { get; set; }
      public string Lastname { get; set; }
      public string ChannelDescription { get; set; }
      public string ProfileImage { get; set; }
      public DateTime DateOfBirth { get; set; }
      public bool Blocked { get; set; }
      public bool Deleted { get; set; }
      //public List<ReactionModel> Reactions { get; set; }
      public List<VideoModel> Videos { get; set; }
      public List<NotificationModel> Notifications { get; set; }
      public List<SubscriptionModel> Subscribers { get; set; }
      public List<SubscriptionModel> SubscribedOn { get; set; }
   }
}
