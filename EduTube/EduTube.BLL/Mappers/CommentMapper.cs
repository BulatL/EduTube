using EduTube.BLL.Models;
using EduTube.DAL.Entities;
using System.Collections.Generic;
using System.Linq;

namespace EduTube.BLL.Mappers
{
    public class CommentMapper
    {

        public static CommentModel EntityToModel(Comment entity)
        {
            if (entity == null)
                return null;

            CommentModel model  = new CommentModel();
            model.Id            = entity.Id;
            model.UserId        = entity.UserId;
            model.VideoId       = entity.VideoId;
            model.Content       = entity.Content;
            model.DateCreatedOn = entity.DateCreatedOn;
            model.Deleted       = entity.Deleted;

            if (entity.Video != null)
            {
                entity.Video.Comments = null;
                model.Video = VideoMapper.EntityToModel(entity.Video);
            }

            if (entity.User != null)
                model.User = UserMapper.EntityToModel(entity.User);

            if (entity.Reactions != null)
            {
                entity.Reactions.Select(x => x.Comment = null);
                model.Reactions = ReactionMapper.EntitiesToModels(entity.Reactions);
            }

            return model;
        }
        public static Comment ModelToEntity(CommentModel model)
        {
            Comment entity       = new Comment();
            entity.Id            = model.Id;
            entity.UserId        = model.UserId;
            entity.VideoId       = model.VideoId;
            entity.Content       = model.Content;
            entity.DateCreatedOn = model.DateCreatedOn;
            entity.Deleted       = model.Deleted;

            if (model.Video != null)
            {
                model.Video = null;
                entity.Video = VideoMapper.ModelToEntity(model.Video);
            }

            if (model.User != null)
                entity.User = UserMapper.ModelToEntity(model.User);
            

            if (model.Reactions != null)
            {
                model.Reactions.Select(x => x.Comment = null);
                entity.Reactions = ReactionMapper.ModelsToEntities(model.Reactions);
            }

            return entity;
        }

        public static List<CommentModel> EntitiesToModels(IEnumerable<Comment> entities)
        {
            List<CommentModel> models = entities.Select(x => EntityToModel(x)).ToList();
            return models;
        }

        public static List<Comment> ModelsToEntities(List<CommentModel> models)
        {
            List<Comment> entities = models.Select(x => ModelToEntity(x)).ToList();
            return entities;
        }
    }
}