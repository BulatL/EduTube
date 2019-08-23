using EduTube.BLL.Models;
using EduTube.DAL.Entities;
using System.Collections.Generic;
using System.Linq;

namespace EduTube.BLL.Mappers
{
   public class HashtagRelationshipMapper
   {

      public static HashtagRelationshipModel EntityToModel(HashTagRelationship entity)
      {
         if (entity == null)
            return null;

         HashtagRelationshipModel model = new HashtagRelationshipModel();
         model.Id = entity.Id;
         model.ChatId = entity.ChatId;
         model.VideoId = entity.VideoId;
         model.HashTagId = entity.HashTagId;

         if (entity.Chat != null)
         {
            entity.Chat.Hashtags.Select(x => x.Chat = null);
            model.Chat = ChatMapper.EntityToModel(entity.Chat);
         }

         if (entity.Video != null)
         {
            entity.Video.HashtagRelationships.Select(x => x.Video = null);
            model.Video = VideoMapper.EntityToModel(entity.Video);
         }

         if (entity.Hashtag != null)
            model.Hashtag = HashtagMapper.EntityToModel(entity.Hashtag);

         return model;
      }
      public static HashTagRelationship ModelToEntity(HashtagRelationshipModel model)
      {
         HashTagRelationship entity = new HashTagRelationship();
         entity.Id = model.Id;
         entity.ChatId = model.ChatId;
         entity.VideoId = model.VideoId;
         entity.HashTagId = model.HashTagId;

         if (model.Chat != null)
         {
            model.Chat.Hashtags.Select(x => x.Chat = null);
            entity.Chat = ChatMapper.ModelToEntity(model.Chat);
         }

         if (model.Video != null)
         {
            model.Video.HashtagRelationships.Select(x => x.Video = null);
            entity.Video = VideoMapper.ModelToEntity(model.Video);
         }
         if (model.Hashtag != null)
            entity.Hashtag = HashtagMapper.ModelToEntity(model.Hashtag);

         return entity;
      }

      public static List<HashtagRelationshipModel> EntitiesToModels(IEnumerable<HashTagRelationship> entities)
      {
         List<HashtagRelationshipModel> models = entities.Select(x => EntityToModel(x)).ToList();
         return models;
      }

      public static List<HashTagRelationship> ModelsToEntities(List<HashtagRelationshipModel> models)
      {
         List<HashTagRelationship> entities = models.Select(x => ModelToEntity(x)).ToList();
         return entities;
      }
   }
}
