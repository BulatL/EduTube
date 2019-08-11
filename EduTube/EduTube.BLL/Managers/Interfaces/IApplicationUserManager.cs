using EduTube.BLL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EduTube.BLL.Managers.Interfaces
{
    public interface IApplicationUserManager
    {
        Task<List<ApplicationUserModel>> GetAll();
        Task<ApplicationUserModel> GetById(string id, bool includeAll);
        Task<ApplicationUserModel> GetByChannelName(string channelName);
        //ApplicationUserModel Create(int id);
        Task<ApplicationUserModel> Update(ApplicationUserModel userModel);
        Task Delete(string id);
        Task Activate(string id);
    }
}
