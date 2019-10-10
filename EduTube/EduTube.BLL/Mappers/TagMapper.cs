using EduTube.BLL.Models;
using EduTube.DAL.Entities;
using System.Collections.Generic;
using System.Linq;
namespace EduTube.BLL.Mappers
{
   public class TagMapper
   {
      public static void CopyModelToEntity(TagModel model, Tag entity)
      {
         entity.Id = model.Id;
         entity.Name = model.Name;
      }

      public static TagModel EntityToModel(Tag entity)
      {
         if (entity == null)
            return null;

         TagModel model = new TagModel();
         model.Id = entity.Id;
         model.Name = entity.Name;

         return model;
      }
      public static Tag ModelToEntity(TagModel model)
      {
         Tag entity = new Tag();
         entity.Id = model.Id;
         entity.Name = model.Name;

         return entity;
      }

      public static List<TagModel> EntitiesToModels(IEnumerable<Tag> entities)
      {
         List<TagModel> models = entities.Select(x => EntityToModel(x)).ToList();
         return models;
      }

      public static List<Tag> ModelsToEntities(List<TagModel> models)
      {
         List<Tag> entities = models.Select(x => ModelToEntity(x)).ToList();
         return entities;
      }
   }
}
