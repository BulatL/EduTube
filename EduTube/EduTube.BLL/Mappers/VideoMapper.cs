using EduTube.BLL.Models;
using EduTube.DAL.Entities;
using System.Collections.Generic;
using System.Linq;

namespace EduTube.BLL.Mappers
{
    public class VideoMapper
    {
        public static VideoModel EntityToModel(Video entity)
        {
            if (entity == null)
                return null;

            VideoModel model = new VideoModel();
            model.Id              = entity.Id;
            model.Name            = entity.Name;
            model.UserId          = entity.UserId;
            model.Blocked         = entity.Blocked;
            model.Deleted         = entity.Deleted;
            model.FilePath        = entity.FilePath;
            model.Duration        = entity.Duration;
            model.Thumbnail       = entity.Thumbnail;
            model.IvniteCode      = entity.IvniteCode;
            model.YoutubeUrl      = entity.YoutubeUrl;
            model.Description     = entity.Description;
            model.DateCreatedOn   = entity.DateCreatedOn;
            model.AllowComments   = entity.AllowComments;
            model.VideoVisibility = VideoVisibilityMapper.EntityToModel(entity.VideoVisibility);

            if (entity.User != null)
            {
                entity.User.Videos = null;
                model.UserChannelName = entity.User.ChannelName;
                model.User = UserMapper.EntityToModel(entity.User);
            }

            if (entity.HashtagRelationships != null)
            {
                entity.HashtagRelationships.ForEach(x => x.Video = null);
                model.HashtagRelationships = HashtagRelationshipMapper.EntitiesToModels(entity.HashtagRelationships);
                model.Hashtags = string.Join(",", entity.HashtagRelationships.Select(x => x.Hashtag).Select(x => x.Name));
            }

            if (entity.Comments != null)
            {
                entity.Comments.ForEach(x => x.Video = null);
                model.Comments = CommentMapper.EntitiesToModels(entity.Comments);
            }

            if (entity.Views != null)
            {
                entity.Views.ForEach(x => x.Video = null);
                model.Views = ViewMapper.EntitiesToModels(entity.Views);
            }

            if (entity.Reactions != null)
            {
                entity.Reactions.ForEach(x => x.Video = null);
                model.Reactions = ReactionMapper.EntitiesToModels(entity.Reactions);
            }

            return model;
        }
        public static Video ModelToEntity(VideoModel model)
        {
            Video entity = new Video();
            entity.Id              = model.Id;
            entity.Name            = model.Name;
            entity.UserId          = model.UserId;
            entity.Blocked         = model.Blocked;
            entity.Deleted         = model.Deleted;
            entity.FilePath        = model.FilePath;
            entity.Duration        = model.Duration;
            entity.Thumbnail       = model.Thumbnail;
            entity.IvniteCode      = model.IvniteCode;
            entity.YoutubeUrl      = model.YoutubeUrl;
            entity.Description     = model.Description;
            entity.DateCreatedOn   = model.DateCreatedOn;
            entity.AllowComments   = model.AllowComments;
            entity.VideoVisibility = VideoVisibilityMapper.ModelToEntity(model.VideoVisibility);

            if (model.User != null)
            {
                model.User.Videos = null;
                entity.User = UserMapper.ModelToEntity(model.User);
            }

            if (model.HashtagRelationships != null)
            {
                model.HashtagRelationships.Select(x => x.Video = null);
                entity.HashtagRelationships = HashtagRelationshipMapper.ModelsToEntities(model.HashtagRelationships);
            }

            if (model.Comments != null)
            {
                model.Comments.Select(x => x.Video = null);
                entity.Comments = CommentMapper.ModelsToEntities(model.Comments);
            }

            if (model.Views != null)
            {
                model.Views.Select(x => x.Video = null);
                entity.Views = ViewMapper.ModelsToEntities(model.Views);
            }

            return entity;
        }

        public static List<VideoModel> EntitiesToModels(IEnumerable<Video> entities)
        {
            List<VideoModel> models = entities.Select(x => EntityToModel(x)).ToList();
            return models;
        }

        public static List<Video> ModelsToEntities(List<VideoModel> models)
        {
            List<Video> entities = models.Select(x => ModelToEntity(x)).ToList();
            return entities;
        }
    }
}
