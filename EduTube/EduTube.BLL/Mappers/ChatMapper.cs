using EduTube.BLL.Models;
using EduTube.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EduTube.BLL.Mappers
{
    public class ChatMapper
    {
        public static ChatModel EntityToModel(Chat entity)
        {
            if (entity == null)
                return null;

            ChatModel model = new ChatModel();
            model.Id = entity.Id;
            model.Name = entity.Name;
            model.Deleted = entity.Deleted;

            if (entity.Hashtags != null)
            {
                entity.Hashtags.ForEach(x => x.Chat = null);
                model.Hashtags = HashtagRelationshipMapper.EntitiesToModels(entity.Hashtags);
            }

            if (entity.Messages != null)
            {
                entity.Messages.ForEach(x => x.Chat = null);
                model.Messages = ChatMessageMapper.EntitiesToModels(entity.Messages);
            }

            /*if (entity.Messages != null)
                model.Messages = ChatMessageMapper.EntitiesToModels(entity.Messages);*/

            return model;
        }
        public static Chat ModelToEntity(ChatModel model)
        {
            Chat entity    = new Chat();
            entity.Id      = model.Id;
            entity.Name    = model.Name;
            entity.Deleted = model.Deleted;

            /*if (model.Hashtags != null)
            {
                model.Hashtags.ForEach(x => x.Chat = null);
                entity.Hashtags = HashtagRelationshipMapper.ModelsToEntities(model.Hashtags);
            }

            if (model.Messages != null)
            {
                model.Messages.ForEach(x => x.Chat = null);
                entity.Messages = ChatMessageMapper.ModelsToEntities(model.Messages);
            }*/

            return entity;
        }

        public static List<ChatModel> EntitiesToModels(IEnumerable<Chat> entities)
        {
            List<ChatModel> models = entities.Select(x => EntityToModel(x)).ToList();
            return models;
        }

        public static List<Chat> ModelsToEntities(List<ChatModel> models)
        {
            List<Chat> entities = models.Select(x => ModelToEntity(x)).ToList();
            return entities;
        }
    }
}
