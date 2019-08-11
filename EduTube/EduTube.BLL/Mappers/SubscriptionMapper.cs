using EduTube.BLL.Models;
using EduTube.DAL.Entities;
using System.Collections.Generic;
using System.Linq;

namespace EduTube.BLL.Mappers
{
    public class SubscriptionMapper
    {

        public static SubscriptionModel EntityToModel(Subscription entity)
        {
            if (entity == null)
                return null;

            SubscriptionModel model = new SubscriptionModel();
            model.Id             = entity.Id;
            model.SubscriberId   = entity.SubscriberId;
            model.ExpirationDate = entity.ExpirationDate;
            model.SubscribedOnId = entity.SubscribedOnId;
            model.Deleted        = entity.Deleted;

            if (entity.SubscribedOn != null)
            {
                entity.SubscribedOn.SubscribedOn = null;
                entity.SubscribedOn.Subscribers = null;
                model.SubscribedOn = UserMapper.EntityToModel(entity.SubscribedOn);
            }

            if (entity.Subscriber != null)
            {
                entity.Subscriber.SubscribedOn = null;
                entity.Subscriber.Subscribers = null;
                model.Subscriber = UserMapper.EntityToModel(entity.Subscriber);
            }

            return model;
        }
        public static Subscription ModelToEntity(SubscriptionModel model)
        {
            Subscription entity   = new Subscription();
            entity.Id             = model.Id;
            entity.SubscriberId   = model.SubscriberId;
            entity.ExpirationDate = model.ExpirationDate;
            entity.SubscribedOnId = model.SubscribedOnId;
            entity.Deleted        = model.Deleted;

            if (model.SubscribedOn != null)
            {
                model.SubscribedOn.SubscribedOn = null;
                model.SubscribedOn.Subscribers = null;
                entity.SubscribedOn = UserMapper.ModelToEntity(model.SubscribedOn);
            }

            if (entity.Subscriber != null)
            {
                model.Subscriber.SubscribedOn = null;
                model.Subscriber.Subscribers = null;
                entity.Subscriber = UserMapper.ModelToEntity(model.Subscriber);
            }

            return entity;
        }

        public static List<SubscriptionModel> EntitiesToModels(IEnumerable<Subscription> entities)
        {
            List<SubscriptionModel> models = entities.Select(x => EntityToModel(x)).ToList();
            return models;
        }

        public static List<Subscription> ModelsToEntities(List<SubscriptionModel> models)
        {
            List<Subscription> entities = models.Select(x => ModelToEntity(x)).ToList();
            return entities;
        }
    }
}
