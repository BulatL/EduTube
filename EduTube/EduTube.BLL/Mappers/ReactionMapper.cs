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
         model.EmojiId = entity.EmojiId;
         model.DateCreatedOn = entity.DateCreatedOn;

         if (entity.Emoji != null)
            model.Emoji = EmojiMapper.EntityToModel(entity.Emoji);

         if (entity.User != null)
            model.User = UserMapper.EntityToModel(entity.User);

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
         entity.EmojiId = model.EmojiId;
         entity.DateCreatedOn = model.DateCreatedOn;

         if (model.Emoji != null)
            entity.Emoji = EmojiMapper.ModelToEntity(model.Emoji);


         if (model.User != null)
            entity.User = UserMapper.ModelToEntity(model.User);

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
