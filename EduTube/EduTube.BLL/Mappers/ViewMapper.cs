using EduTube.BLL.Models;
using EduTube.DAL.Entities;
using System.Collections.Generic;
using System.Linq;

namespace EduTube.BLL.Mappers
{
   public class ViewMapper
   {
      public static ViewModel EntityToModel(View entity)
      {
         if (entity == null)
            return null;

         ViewModel model = new ViewModel();
         model.Id = entity.Id;
         model.UserId = entity.UserId;
         model.VideoId = entity.VideoId;
         model.IpAddress = entity.IpAddress;

         return model;
      }
      public static View ModelToEntity(ViewModel model)
      {
         View entity = new View();
         entity.Id = model.Id;
         entity.UserId = model.UserId;
         entity.VideoId = model.VideoId;
         entity.IpAddress = model.IpAddress;

         return entity;
      }

      public static List<ViewModel> EntitiesToModels(List<View> entities)
      {
         List<ViewModel> models = entities.Select(x => EntityToModel(x)).ToList();
         return models;
      }

      public static List<View> ModelsToEntities(List<ViewModel> models)
      {
         List<View> entities = models.Select(x => ModelToEntity(x)).ToList();
         return entities;
      }
   }
}