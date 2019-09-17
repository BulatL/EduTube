﻿using EduTube.BLL.Models;
using EduTube.DAL.Entities;
using System.Collections.Generic;
using System.Linq;

namespace EduTube.BLL.Mappers
{
   public class NotificationMapper
   {

      public static NotificationModel EntityToModel(Notification entity)
      {
         if (entity == null)
            return null;

         NotificationModel model = new NotificationModel();
         model.Id = entity.Id;
         model.UserId = entity.UserId;
         model.Content = entity.Content;
         model.Seen = entity.Seen;
         model.Deleted = entity.Deleted;
         model.DateCreatedOn = entity.DateCreatedOn;


         return model;
      }
      public static Notification ModelToEntity(NotificationModel model)
      {
         Notification entity = new Notification();
         entity.Id = model.Id;
         entity.UserId = model.UserId;
         entity.Content = model.Content;
         entity.Seen = model.Seen;
         entity.Deleted = model.Deleted;
         entity.DateCreatedOn = model.DateCreatedOn;

         return entity;
      }

      public static List<NotificationModel> EntitiesToModels(IEnumerable<Notification> entities)
      {
         List<NotificationModel> models = entities.Select(x => EntityToModel(x)).ToList();
         return models;
      }

      public static List<Notification> ModelsToEntities(List<NotificationModel> models)
      {
         List<Notification> entities = models.Select(x => ModelToEntity(x)).ToList();
         return entities;
      }
   }
}
