using EduTube.BLL.Models;
using EduTube.DAL.Entities;
using System.Collections.Generic;
using System.Linq;
namespace EduTube.BLL.Mappers
{
   public class HashtagMapper
   {

      public static HashtagModel EntityToModel(Hashtag entity)
      {
         if (entity == null)
            return null;

         HashtagModel model = new HashtagModel();
         model.Id = entity.Id;
         model.Name = entity.Name;

         return model;
      }
      public static Hashtag ModelToEntity(HashtagModel model)
      {
         Hashtag entity = new Hashtag();
         entity.Id = model.Id;
         entity.Name = model.Name;

         return entity;
      }

      public static List<HashtagModel> EntitiesToModels(IEnumerable<Hashtag> entities)
      {
         List<HashtagModel> models = entities.Select(x => EntityToModel(x)).ToList();
         return models;
      }

      public static List<Hashtag> ModelsToEntities(List<HashtagModel> models)
      {
         List<Hashtag> entities = models.Select(x => ModelToEntity(x)).ToList();
         return entities;
      }
   }
}
