using EduTube.BLL.Models;
using EduTube.DAL.Entities;
using System.Collections.Generic;
using System.Linq;

namespace EduTube.BLL.Mappers
{
    public class EmoticonMapper
    {

        public static EmoticonModel EntityToModel(Emoticon entity)
        {
            if (entity == null)
                return null;

            EmoticonModel model = new EmoticonModel();
            model.Id        = entity.Id;
            model.Name      = entity.Name;
            model.ImagePath = entity.ImagePath;
            model.Deleted   = entity.Deleted;

            return model;
        }
        public static Emoticon ModelToEntity(EmoticonModel model)
        {
            Emoticon entity = new Emoticon();
            entity.Id        = model.Id;
            entity.Name      = model.Name;
            entity.ImagePath = model.ImagePath;
            entity.Deleted  = model.Deleted;

            return entity;
        }

        public static List<EmoticonModel> EntitiesToModels(IEnumerable<Emoticon> entities)
        {
            List<EmoticonModel> models = entities.Select(x => EntityToModel(x)).ToList();
            return models;
        }

        public static List<Emoticon> ModelsToEntities(List<EmoticonModel> models)
        {
            List<Emoticon> entities = models.Select(x => ModelToEntity(x)).ToList();
            return entities;
        }
    }
}
