using System.Collections.Generic;
using System.Threading.Tasks;
using EduTube.BLL.Models;

namespace EduTube.BLL.Managers.Interfaces
{
    public interface ISubscriptionManager
    {
        Task<List<SubscriptionModel>> GetAll();
        Task<SubscriptionModel> GetById(int id, bool includeAll);
        Task<SubscriptionModel> Create(SubscriptionModel subscription);
        Task<SubscriptionModel> Update(SubscriptionModel subscription);
        Task Delete(int id);
    }
}
