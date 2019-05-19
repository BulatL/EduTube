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
            ViewModel model = new ViewModel();
            entity.Id        = model.Id;
            entity.UserId    = model.UserId;
            entity.VideoId   = model.VideoId;
            entity.IpAddress = model.IpAddress;

            if (entity.User != null)
                model.User = UserMapper.EntityToModel(entity.User);

            if (entity.Video != null)
            {
                entity.Video.Views = null;
                model.Video = VideoMapper.EntityToModel(entity.Video);
            }

            return model;
        }
        public static View ModelToEntity(ViewModel model)
        {
            View entity = new View();
            entity.Id        = model.Id;
            entity.UserId    = model.UserId;
            entity.VideoId   = model.VideoId;
            entity.IpAddress = model.IpAddress;

            if (model.User != null)
                entity.User = UserMapper.ModelToEntity(model.User);

            if (model.Video != null)
            {
                model.Video.Views = null;
                entity.Video = VideoMapper.ModelToEntity(model.Video);
            }

            return entity;
        }

        public static List<ViewModel> EntitiesToModels(IEnumerable<View> entities)
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