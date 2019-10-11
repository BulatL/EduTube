using System.Collections.Generic;
using System.Threading.Tasks;
using EduTube.BLL.Models;

namespace EduTube.BLL.Managers.Interfaces
{
   public interface ISubscriptionManager
   {
      Task<bool> IsUserSubscribed(string subscribedOn, string subscriber);
      Task<SubscriptionModel> Create(SubscriptionModel subscription);
      Task Remove(string subscriberId, string subscribedOnId);
   }
}
