using EduTube.BLL.Models;
using EduTube.DAL.Entities;
using System.Collections.Generic;
using System.Linq;

namespace EduTube.BLL.Mappers
{
   public class ReactionMapper
   {

      public static ReactionModel EntityToModel(Reaction entity)
      {
         if (entity == null)
            return null;

         ReactionModel model = new ReactionModel();
         model.Id = entity.Id;
         model.UserId = entity.UserId;
         model.Deleted = entity.Deleted;
         model.VideoId = entity.VideoId;
         model.CommentId = entity.CommentId;
         model.EmoticonId = entity.EmoticonId;
         model.DateCreatedOn = entity.DateCreatedOn;

         if (entity.Emoticon != null)
            model.Emoticon = EmoticonMapper.EntityToModel(entity.Emoticon);

         if (entity.User != null)
            model.User = UserMapper.EntityToModel(entity.User);

         /*if (entity.Video != null)
         {
            entity.Video.Reactions = null;
            model.Video = VideoMapper.EntityToModel(entity.Video);
         }

         if (entity.Comment != null)
         {
            entity.Comment.Reactions = null;
            model.Comment = CommentMapper.EntityToModel(entity.Comment);
         }*/

         return model;
      }
      public static Reaction ModelToEntity(ReactionModel model)
      {
         Reaction entity = new Reaction();
         entity.Id = model.Id;
         entity.UserId = model.UserId;
         entity.Deleted = model.Deleted;
         entity.VideoId = model.VideoId;
         entity.CommentId = model.CommentId;
         entity.EmoticonId = model.EmoticonId;
         entity.DateCreatedOn = model.DateCreatedOn;

         if (model.Emoticon != null)
            entity.Emoticon = EmoticonMapper.ModelToEntity(model.Emoticon);


         if (model.User != null)
            entity.User = UserMapper.ModelToEntity(model.User);

         /*if (model.Video != null)
         {
            model.Video.Reactions = null;
            entity.Video = VideoMapper.ModelToEntity(model.Video);
         }

         if (model.Comment != null)
         {
            model.Comment.Reactions = null;
            entity.Comment = CommentMapper.ModelToEntity(model.Comment);
         }*/

         return entity;
      }

      public static List<ReactionModel> EntitiesToModels(IEnumerable<Reaction> entities)
      {
         List<ReactionModel> models = entities.Select(x => EntityToModel(x)).ToList();
         return models;
      }

      public static List<Reaction> ModelsToEntities(List<ReactionModel> models)
      {
         List<Reaction> entities = models.Select(x => ModelToEntity(x)).ToList();
         return entities;
      }
   }
}
