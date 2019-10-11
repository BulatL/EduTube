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

      public async Task<bool> IsUserSubscribed(string subscribedOn, string subscriber)
      {
         return (await _context.Subscriptions.AnyAsync(x => x.SubscribedOnId.Equals(subscribedOn) && x.SubscriberId.Equals(subscriber) && !x.Deleted));
      }

      public async Task<SubscriptionModel> Create(SubscriptionModel subscription)
      {
         Subscription exist = await _context.Subscriptions.FirstOrDefaultAsync(x => x.SubscribedOnId.Equals(subscription.SubscribedOnId)
         && x.SubscriberId.Equals(subscription.SubscriberId) && x.Deleted);
         if (exist != null)
         {
            exist.Deleted = false;
            await _context.SaveChangesAsync();
            return SubscriptionMapper.EntityToModel(exist);
         }
         else
         {
            Subscription entity = SubscriptionMapper.ModelToEntity(subscription);
            _context.Subscriptions.Add(entity);
            await _context.SaveChangesAsync();
            return SubscriptionMapper.EntityToModel(entity);
         }
      }

      public async Task Remove(string subscriberId, string subscribedOnId)
      {
         Subscription entity = await _context.Subscriptions.FirstOrDefaultAsync(x => x.SubscriberId.Equals(subscriberId) && x.SubscribedOnId.Equals(subscribedOnId) && !x.Deleted);
         _context.Subscriptions.Remove(entity);
         await _context.SaveChangesAsync();
      }

   }
}
