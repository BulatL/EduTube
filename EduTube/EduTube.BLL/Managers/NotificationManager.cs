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

      public async Task<List<NotificationModel>> GetAll()
      {
         return NotificationMapper.EntitiesToModels(await _context.Notifications.Where(x => !x.Deleted).ToListAsync());
      }

      public async Task<NotificationModel> GetById(int id)
      {
         return NotificationMapper.EntityToModel(await _context.Notifications
             .FirstOrDefaultAsync(x => x.Id == id && !x.Deleted));
      }

      public async Task<NotificationModel> Create(NotificationModel notification)
      {
         Notification entity = NotificationMapper.ModelToEntity(notification);
         _context.Notifications.Add(entity);
         await _context.SaveChangesAsync();
         return NotificationMapper.EntityToModel(entity);
      }

      public async Task<NotificationModel> Update(NotificationModel notification)
      {
         _context.Update(NotificationMapper.ModelToEntity(notification));
         await _context.SaveChangesAsync();
         return notification;
      }

      public async Task Delete(int id)
      {
         Notification entity = await _context.Notifications.FirstOrDefaultAsync(x => x.Id == id);
         /*entity.Deleted = true;
         _context.Update(entity);*/
         _context.Notifications.Remove(entity);
         await _context.SaveChangesAsync();
      }
   }
}
