using System.Collections.Generic;
using System.Threading.Tasks;
using EduTube.BLL.Models;

namespace EduTube.BLL.Managers.Interfaces
{
   public interface ISubscriptionManager
   {
      Task<List<SubscriptionModel>> GetAll();
      Task<SubscriptionModel> GetById(int id, bool includeAll);
      Task<List<SubscriptionModel>> GetBySubscriber(string id);
      Task<List<SubscriptionModel>> GetBySubscribedOn(string id);
      Task<SubscriptionModel> GetBySubscriberAndSubscribedOn(string subscriberId, string subscribedOnId);
      Task<SubscriptionModel> Create(SubscriptionModel subscription);
      Task<SubscriptionModel> Update(SubscriptionModel subscription);
      Task Delete(string subscriberId, string subscribedOnId);
      Task Remove(string subscriberId, string subscribedOnId);
      Task DeleteActivateByUser(string id, bool option);
   }
}
