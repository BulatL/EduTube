using EduTube.BLL.Models;
using EduTube.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EduTube.BLL.Mappers
{
   public class ChatMessageMapper
   {
      public static ChatMessageModel EntityToModel(ChatMessage entity)
      {
         if (entity == null)
            return null;

         ChatMessageModel model = new ChatMessageModel();
         model.Id = entity.Id;
         model.UserId = entity.UserId;
         model.ChatId = entity.ChatId;
         model.Message = entity.Message;
         model.Deleted = entity.Deleted;
         model.DateCreatedOn = entity.DateCreatedOn;

         if (entity.User != null)
         {
            model.User = UserMapper.EntityToModel(entity.User);
         }

         return model;
      }
      public static ChatMessage ModelToEntity(ChatMessageModel model)
      {
         ChatMessage entity = new ChatMessage();
         entity.Id = model.Id;
         entity.UserId = model.UserId;
         entity.ChatId = model.ChatId;
         entity.Message = model.Message;
         entity.Deleted = model.Deleted;
         entity.DateCreatedOn = model.DateCreatedOn;

         if (model.User != null)
         {
            entity.User = UserMapper.ModelToEntity(model.User);
         }

         return entity;
      }

      public static List<ChatMessageModel> EntitiesToModels(IEnumerable<ChatMessage> entities)
      {
         List<ChatMessageModel> models = entities.Select(x => EntityToModel(x)).ToList();
         return models;
      }

      public static List<ChatMessage> ModelsToEntities(List<ChatMessageModel> models)
      {
         List<ChatMessage> entities = models.Select(x => ModelToEntity(x)).ToList();
         return entities;
      }
   }
}
