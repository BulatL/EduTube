using EduTube.BLL.Managers.Interfaces;
using EduTube.BLL.Models;
using EduTube.DAL.Data;
using EduTube.BLL.Mappers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduTube.DAL.Entities;

namespace EduTube.BLL.Managers
{
    public class SubscriptionManager : ISubscriptionManager
    {
        private ApplicationDbContext _context;

        public SubscriptionManager(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<SubscriptionModel>> GetAll()
        {
            return SubscriptionMapper.EntitiesToModels(await _context.Subscriptions.Where(x => !x.Deleted).ToListAsync());
        }

        public async Task<SubscriptionModel> GetById(int id, bool includeAll)
        {
            return includeAll ? SubscriptionMapper.EntityToModel(await _context.Subscriptions
                .Include(x => x.SubscribedOn).Include(x => x.Subscriber)
                .FirstOrDefaultAsync(x => x.Id == id && !x.Deleted))
                : SubscriptionMapper.EntityToModel(await _context.Subscriptions
                .FirstOrDefaultAsync(x => x.Id == id && !x.Deleted));
        }

        public async Task<List<SubscriptionModel>> GetBySubscriber(string id)
        {
            return SubscriptionMapper.EntitiesToModels(await _context.Subscriptions.Where(x => x.SubscriberId.Equals(id) && !x.Deleted).ToListAsync());
        }

        public async Task<List<SubscriptionModel>> GetBySubscribedOn(string id)
        {
            return SubscriptionMapper.EntitiesToModels(await _context.Subscriptions.Where(x => x.SubscribedOnId.Equals(id) && !x.Deleted).ToListAsync());
        }

        public async Task<SubscriptionModel> GetBySubscriberAndSubscribedOn(string subscriberId, string subscribedOnId)
        {
            return SubscriptionMapper.EntityToModel(await _context.Subscriptions.FirstOrDefaultAsync(x => x.SubscriberId.Equals(subscriberId) && x.SubscribedOnId.Equals(subscribedOnId) && !x.Deleted));
        }

        public async Task<SubscriptionModel> Create(SubscriptionModel subscription)
        {
            Subscription entity = SubscriptionMapper.ModelToEntity(subscription);
            _context.Subscriptions.Add(entity);
            await _context.SaveChangesAsync();
            return SubscriptionMapper.EntityToModel(entity);
        }

        public async Task<SubscriptionModel> Update(SubscriptionModel subscription)
        {
            _context.Update(SubscriptionMapper.ModelToEntity(subscription));
            await _context.SaveChangesAsync();
            return subscription;
        }

        public async Task Delete(string subscriberId, string subscribedOnId)
        {
            Subscription entity = await _context.Subscriptions.FirstOrDefaultAsync(x => x.SubscriberId.Equals(subscriberId) && x.SubscribedOnId.Equals(subscribedOnId) && !x.Deleted);
            entity.Deleted = true;
            _context.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Remove(string subscriberId, string subscribedOnId)
        {
            Subscription entity = await _context.Subscriptions.FirstOrDefaultAsync(x => x.SubscriberId.Equals(subscriberId) && x.SubscribedOnId.Equals(subscribedOnId) && !x.Deleted);
            _context.Subscriptions.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
