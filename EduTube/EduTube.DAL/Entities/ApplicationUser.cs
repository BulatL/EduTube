using EduTube.DAL.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduTube.DAL.Entities
{
   public class ApplicationUser : IdentityUser
   {
      //public int Id { get; set; }
      //public string Username { get; set; }
      //public string Password { get; set; }
      //public string Email { get; set; }
      public string ChannelName { get; set; }
      public string Firstname { get; set; }
      public string Lastname { get; set; }
      public string ChannelDescription { get; set; }
      public string ProfileImage { get; set; }
      public DateTime DateOfBirth { get; set; }
      public bool Blocked { get; set; }
      public bool Deleted { get; set; }
      //public virtual List<Reaction> Reactions { get; set; }
      public List<Video> Videos { get; set; }
      public List<Notification> Notifications { get; set; }
      public List<Subscription> Subscribers { get; set; }
      public List<Subscription> SubscribedOn { get; set; }
   }
}
