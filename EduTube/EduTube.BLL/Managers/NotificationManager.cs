using EduTube.BLL.Managers.Interfaces;
using EduTube.BLL.Models;
using EduTube.DAL.Data;
using EduTube.BLL.Mappers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduTube.DAL.Entities;

namespace EduTube.BLL.Managers
{
   public class NotificationManager : INotificationManager
   {
      private ApplicationDbContext _context;

      public NotificationManager(ApplicationDbContext context)
      {
         _context = context;
      }

      public async Task<List<NotificationModel>> Get5ByUser(string userId, int skip)
      {
         return NotificationMapper.EntitiesToModels(await _context.Notifications.Where(x => !x.Deleted && x.UserId.Equals(userId))
            .OrderByDescending(x => x.DateCreatedOn).Skip(skip).Take(5).ToListAsync());
      }

      public async Task<List<NotificationModel>> GetLast5ByUser(string userId)
      {
         return NotificationMapper.EntitiesToModels(await _context.Notifications.Where(x => !x.Deleted && x.UserId.Equals(userId))
            .OrderByDescending(x => x.DateCreatedOn).Take(5).ToListAsync());
      }
      
      public async Task<List<NotificationModel>> GetNewNotifications(string userId, int lastId)
      {
         return NotificationMapper.EntitiesToModels(await _context.Notifications.Where(x => !x.Deleted && x.UserId.Equals(userId) && x.Id > lastId)
            .OrderByDescending(x => x.DateCreatedOn).ToListAsync());
      }

      public async Task CreateByVideo(string userName, string userId, string userImg, string videoName, int videoId, DateTime date)
      {
         var message = String.Format(@"<span><a href='/Users/{0}' class='boldTxt'>{1}</a> just uploaded new video <a href='/Videos/{2}' class='boldTxt'>{3}</a> <br/> Don't forget to check it.</span>", userName.Replace(" ", "-"), userName, videoId, videoName);
         List<Subscription> subscriptions = await _context.Subscriptions.Where(x => x.SubscribedOnId.Equals(userId) && !x.Deleted).ToListAsync();
         List<Notification> notifications = new List<Notification>();
         foreach (var subscription in subscriptions)
         {
            Notification notification = new Notification()
            {
               Content = message,
               Deleted = false,
               DateCreatedOn = date,
               UserProfileImage = userImg,
               UserId = subscription.SubscriberId
            };
            notifications.Add(notification);
         }
         await _context.AddRangeAsync(notifications);
         await _context.SaveChangesAsync();
      }
   }
}
