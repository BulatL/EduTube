using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EduTube.BLL.Models;

namespace EduTube.BLL.Managers.Interfaces
{
   public interface INotificationManager
   {
      Task<List<NotificationModel>> Get5ByUser(string userId, int skip);
      Task<List<NotificationModel>> GetLast5ByUser(string userId);
      Task<List<NotificationModel>> GetNewNotifications(string userId, int lastId);
      Task CreateByVideo(string userName, string userId, string userImg, string videoName, int videoId, DateTime date);
   }
}
