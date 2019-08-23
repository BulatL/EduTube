using EduTube.BLL.Managers;
using EduTube.BLL.Managers.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduTube.BLL.Extensions
{
   public static class ServiceInitializer
   {
      public static void RegisterBLLServices(this IServiceCollection services)
      {
         services.AddScoped<IApplicationUserManager, ApplicationUserManager>();
         services.AddScoped<IChatManager, ChatManager>();
         services.AddScoped<IChatMessageManager, ChatMessageManager>();
         services.AddScoped<ICommentManager, CommentManager>();
         services.AddScoped<IEmoticonManager, EmoticonManager>();
         services.AddScoped<IHashtagManager, HashtagManager>();
         services.AddScoped<IHashtagRelationshipManager, HashtagRelationshipManager>();
         services.AddScoped<INotificationManager, NotificationManager>();
         services.AddScoped<IReactionManager, ReactionManager>();
         services.AddScoped<ISubscriptionManager, SubscriptionManager>();
         services.AddScoped<IVideoManager, VideoManager>();
         services.AddScoped<IViewManager, ViewManager>();
      }
   }
}
