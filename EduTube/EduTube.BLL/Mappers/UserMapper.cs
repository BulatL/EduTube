using EduTube.BLL.Models;
using EduTube.DAL.Entities;
using System.Collections.Generic;
using System.Linq;

namespace EduTube.BLL.Mappers
{
    public class UserMapper
    {
        public static ApplicationUserModel EntityToModel(ApplicationUser entity)
        {
            if (entity == null)
                return null;

            ApplicationUserModel model = new ApplicationUserModel();
            model.AccessFailedCount    = entity.AccessFailedCount;
            model.ConcurrencyStamp     = entity.ConcurrencyStamp;
            model.EmailConfirmed       = entity.EmailConfirmed;
            model.LockoutEnabled       = entity.LockoutEnabled;
            model.LockoutEnd           = entity.LockoutEnd;
            model.NormalizedEmail      = entity.NormalizedEmail;
            model.NormalizedUserName   = entity.NormalizedUserName;
            model.PasswordHash         = entity.PasswordHash;
            model.PhoneNumberConfirmed = entity.PhoneNumberConfirmed;
            model.SecurityStamp        = entity.SecurityStamp;
            model.Id                   = entity.Id;
            model.Blocked              = entity.Blocked;
            model.ChannelDescription   = entity.ChannelDescription;
            model.ChannelName          = entity.ChannelName;
            model.DateOfBirth          = entity.DateOfBirth;
            model.Deleted              = entity.Deleted;
            model.Email                = entity.Email;
            model.Firstname            = entity.Firstname;
            model.Lastname             = entity.Lastname;
            model.PhoneNumber          = entity.PhoneNumber;
            model.ProfileImage         = entity.ProfileImage;
            model.UserName             = entity.UserName;

            if (entity.Videos != null)
            {
                entity.Videos.Select(x => x.User = null);
                model.Videos = VideoMapper.EntitiesToModels(entity.Videos);
            }

            if (entity.Notifications != null)
            {
                entity.Notifications.Select(x => x.User = null);
                model.Notifications = NotificationMapper.EntitiesToModels(entity.Notifications);
            }

            if (entity.SubscribedOn != null)
            {
                foreach (var sub in entity.SubscribedOn)
                {
                    sub.Subscriber = null;
                }
                //entity.SubscribedOn.Select(x => x.SubscribedOn = null);
                //entity.SubscribedOn.Select(x => x.Subscriber = null);
                model.SubscribedOn = SubscriptionMapper.EntitiesToModels(entity.SubscribedOn);
            }

            if (entity.Subscribers != null)
            {
                foreach (var sub in entity.Subscribers)
                {
                    sub.SubscribedOn = null;
                }
                //entity.Subscribers.Select(x => x.SubscribedOn = null);
                //entity.Subscribers.Select(x => x.Subscriber = null);
                model.Subscribers = SubscriptionMapper.EntitiesToModels(entity.Subscribers);
            }

            return model;
        }
        public static ApplicationUser ModelToEntity(ApplicationUserModel model)
        {
            ApplicationUser entity = new ApplicationUser();
            entity.AccessFailedCount    = model.AccessFailedCount;
            entity.ConcurrencyStamp     = model.ConcurrencyStamp;
            entity.EmailConfirmed       = model.EmailConfirmed;
            entity.LockoutEnabled       = model.LockoutEnabled;
            entity.LockoutEnd           = model.LockoutEnd;
            entity.NormalizedEmail      = model.NormalizedEmail;
            entity.NormalizedUserName   = model.NormalizedUserName;
            entity.PasswordHash         = model.PasswordHash;
            entity.PhoneNumberConfirmed = model.PhoneNumberConfirmed;
            entity.SecurityStamp        = model.SecurityStamp;
            entity.Id                   = model.Id;
            entity.Blocked              = model.Blocked;
            entity.ChannelDescription   = model.ChannelDescription;
            entity.ChannelName          = model.ChannelName;
            entity.DateOfBirth          = model.DateOfBirth;
            entity.Deleted              = model.Deleted;
            entity.Email                = model.Email;
            entity.Firstname            = model.Firstname;
            entity.Lastname             = model.Lastname;
            entity.PhoneNumber          = model.PhoneNumber;
            entity.ProfileImage         = model.ProfileImage;
            entity.UserName             = model.UserName;


            if (model.Videos != null)
            {
                model.Videos.ForEach(x => x.User = null);
                entity.Videos = VideoMapper.ModelsToEntities(model.Videos);
            }

            if (model.Notifications != null)
            {
                model.Notifications.ForEach(x => x.User = null);
                entity.Notifications = NotificationMapper.ModelsToEntities(model.Notifications);
            }

            if (model.SubscribedOn != null)
            {
                model.SubscribedOn.ForEach(x => x.SubscribedOn = null);
                model.SubscribedOn.ForEach(x => x.Subscriber = null);
                entity.SubscribedOn = SubscriptionMapper.ModelsToEntities(model.SubscribedOn);
            }

            if (model.Subscribers != null)
            {
                model.Subscribers.ForEach(x => x.SubscribedOn = null);
                model.Subscribers.ForEach(x => x.Subscriber = null);
                entity.Subscribers = SubscriptionMapper.ModelsToEntities(model.Subscribers);
            }

            return entity;
        }

        public static List<ApplicationUserModel> EntitiesToModels(IEnumerable<ApplicationUser> entities)
        {
            List<ApplicationUserModel> models = entities.Select(x => EntityToModel(x)).ToList();
            return models;
        }

        public static List<ApplicationUser> ModelsToEntities(List<ApplicationUserModel> models)
        {
            List<ApplicationUser> entities = models.Select(x => ModelToEntity(x)).ToList();
            return entities;
        }
    }
}
