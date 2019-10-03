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
         /*string message = "<div class='row unSeenNotification padding1'>" +
                              "<div class='col-lg-10 col-md-10 col-sm-12'>" +
                                 "<div class='row'>" +
                                    "<div class='col-lg-1 col-md-2 col-sm-2 marginAuto'>" +
                                       "<a href ='/Users/" + userId + "'> " +
                                          "<div class='notificationUserImageDiv'>" +
                                             "<img src='~/profileImages/" + userImg + "'/> " +
                                          "</div>" +
                                       "</a>" +
                                    "</div>" +
                                    "<div class='col-lg-11 col-md-10 col-sm-10 breakWord marginAuto notificationsContent'>" +
                                       "<a href ='/Users/" + userId + "'>" + userName + "</a> Uploaded new video: <a href='/Videos/" + videoId + "'>" + videoName + ".</a> Don't forget to check it." +
                                    "</div>" +
                                 "</div>" +
                              "</div>" +
                              "<div class='col-lg-2 col-md-2 col-sm-12 marginAuto'>" +
                                 "<span class='float-right' id='dateSpan'>" +
                                    date +
                                 "</span>" +
                              "</div>" +
                           "</div>";*/
         var message = String.Format(@"<h5><a href='/Users/{0}'>{1}</a></h5> just uploaded new video <h5><a href='/Videos/{2}'>{3}</a></h5> <br/> Don't forget to check it.", userName.Replace(" ", "-"), userName, videoId, videoName);
         List<Subscription> subscriptions = await _context.Subscriptions.Where(x => x.SubscribedOnId.Equals(userId) && !x.Deleted).ToListAsync();
         List<Notification> notifications = new List<Notification>();
         foreach (var subscription in subscriptions)
         {
            Notification notification = new Notification()
            {
               Content = message,
               Deleted = false,
               DateCreatedOn = date,
               Seen = false,
               UserId = subscription.SubscriberId
            };
            notifications.Add(notification);
         }
         await _context.AddRangeAsync(notifications);
         await _context.SaveChangesAsync();
      }

      public async Task<NotificationModel> Create(NotificationModel notification)
      {
         Notification entity = NotificationMapper.ModelToEntity(notification);
         _context.Notifications.Add(entity);
         await _context.SaveChangesAsync();
         return NotificationMapper.EntityToModel(entity);
      }
   }
}
