using EduTube.BLL.Models;
using EduTube.DAL.Entities;
using System.Collections.Generic;
using System.Linq;

namespace EduTube.BLL.Mappers
{
   public class EmojiMapper
   {

      public static EmojiModel EntityToModel(Emoji entity)
      {
         if (entity == null)
            return null;

         EmojiModel model = new EmojiModel();
         model.Id = entity.Id;
         model.Name = entity.Name;
         model.ImagePath = entity.ImagePath;
         model.Deleted = entity.Deleted;

         return model;
      }
      public static Emoji ModelToEntity(EmojiModel model)
      {
         Emoji entity = new Emoji();
         entity.Id = model.Id;
         entity.Name = model.Name;
         entity.ImagePath = model.ImagePath;
         entity.Deleted = model.Deleted;

         return entity;
      }

      public static List<EmojiModel> EntitiesToModels(IEnumerable<Emoji> entities)
      {
         List<Models.EmojiModel> models = entities.Select(x => EntityToModel(x)).ToList();
         return models;
      }

      public static List<Emoji> ModelsToEntities(List<EmojiModel> models)
      {
         List<Emoji> entities = models.Select(x => ModelToEntity(x)).ToList();
         return entities;
      }
   }
}
