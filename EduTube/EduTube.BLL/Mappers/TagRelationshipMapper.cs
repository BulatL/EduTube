using EduTube.BLL.Models;
using EduTube.DAL.Entities;
using System.Collections.Generic;
using System.Linq;

namespace EduTube.BLL.Mappers
{
   public class TagRelationshipMapper
   {

      public static void CopyModelToEntity(TagRelationshipModel model, TagRelationship entity)
      {
         if (model == null)
            return;

         entity.ChatId = model.ChatId;
         entity.VideoId = model.VideoId;
         entity.TagId = model.TagId;

         if (model.Tag != null)
         {
            entity.TagId = model.Tag.Id;
            if (entity.Tag == null)
               entity.Tag = new Tag();
            TagMapper.CopyModelToEntity(model.Tag, entity.Tag);
         }

      }
      public static TagRelationshipModel EntityToModel(TagRelationship entity)
      {
         if (entity == null)
            return null;

         TagRelationshipModel model = new TagRelationshipModel();
         model.Id = entity.Id;
         model.ChatId = entity.ChatId;
         model.VideoId = entity.VideoId;
         model.TagId = entity.TagId;

         if (entity.Chat != null)
         {
            entity.Chat.TagRelationships.Select(x => x.Chat = null);
            model.Chat = ChatMapper.EntityToModel(entity.Chat);
         }

         if (entity.Video != null)
         {
            entity.Video.TagRelationships.Select(x => x.Video = null);
            model.Video = VideoMapper.EntityToModel(entity.Video);
         }

         if (entity.Tag != null)
            model.Tag = TagMapper.EntityToModel(entity.Tag);

         return model;
      }
      public static TagRelationship ModelToEntity(TagRelationshipModel model)
      {
         TagRelationship entity = new TagRelationship();
         entity.Id = model.Id;
         entity.ChatId = model.ChatId;
         entity.VideoId = model.VideoId;
         entity.TagId = model.TagId;

         if (model.Chat != null)
         {
            model.Chat.TagRelationships.Select(x => x.Chat = null);
            entity.Chat = ChatMapper.ModelToEntity(model.Chat);
         }

         if (model.Video != null)
         {
            model.Video.TagRelationships.Select(x => x.Video = null);
            entity.Video = VideoMapper.ModelToEntity(model.Video);
         }
         if (model.Tag != null)
            entity.Tag = TagMapper.ModelToEntity(model.Tag);

         return entity;
      }

      public static List<TagRelationshipModel> EntitiesToModels(IEnumerable<TagRelationship> entities)
      {
         List<TagRelationshipModel> models = entities.Select(x => EntityToModel(x)).ToList();
         return models;
      }

      public static List<TagRelationship> ModelsToEntities(List<TagRelationshipModel> models)
      {
         List<TagRelationship> entities = models.Select(x => ModelToEntity(x)).ToList();
         return entities;
      }
   }
}
