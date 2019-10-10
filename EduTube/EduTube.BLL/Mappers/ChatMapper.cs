using EduTube.BLL.Models;
using EduTube.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EduTube.BLL.Mappers
{
   public class ChatMapper
   {
      public static void CopyModelToEntity(ChatModel model, Chat entity)
      {
         if (model == null)
            return;

         entity.Name = model.Name;
         entity.Deleted = model.Deleted;

         if (model.TagRelationships != null)
         {
            for (int i = 0; i < model.TagRelationships.Count; i++)
            {
               if(entity.TagRelationships?.ElementAtOrDefault(i) == null)
               {
                  entity.TagRelationships.Add(new TagRelationship());
               }
               TagRelationshipMapper.CopyModelToEntity(model.TagRelationships.ElementAtOrDefault(i), entity.TagRelationships?.ElementAtOrDefault(i));
            }
         }
      }
      public static ChatModel EntityToModel(Chat entity)
      {
         if (entity == null)
            return null;

         ChatModel model = new ChatModel();
         model.Id = entity.Id;
         model.Name = entity.Name;
         model.Deleted = entity.Deleted;

         if (entity.TagRelationships != null)
         {
            entity.TagRelationships.ForEach(x => x.Chat = null);
            model.TagRelationships = TagRelationshipMapper.EntitiesToModels(entity.TagRelationships);
         }

         if (entity.Messages != null)
         {
            model.Messages = ChatMessageMapper.EntitiesToModels(entity.Messages);
         }

         return model;
      }
      public static Chat ModelToEntity(ChatModel model)
      {
         Chat entity = new Chat();
         entity.Id = model.Id;
         entity.Name = model.Name;
         entity.Deleted = model.Deleted;

         if (model.TagRelationships != null)
         {
             model.TagRelationships.ForEach(x => x.Chat = null);
             entity.TagRelationships = TagRelationshipMapper.ModelsToEntities(model.TagRelationships);
         }
         
         if (model.Messages != null)
         {
             entity.Messages = ChatMessageMapper.ModelsToEntities(model.Messages);
         }

         return entity;
      }

      public static List<ChatModel> EntitiesToModels(IEnumerable<Chat> entities)
      {
         List<ChatModel> models = entities.Select(x => EntityToModel(x)).ToList();
         return models;
      }

      public static List<Chat> ModelsToEntities(List<ChatModel> models)
      {
         List<Chat> entities = models.Select(x => ModelToEntity(x)).ToList();
         return entities;
      }
   }
}
