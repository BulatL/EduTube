using System.Collections.Generic;
using System.Threading.Tasks;
using EduTube.BLL.Models;

namespace EduTube.BLL.Managers.Interfaces
{
    public interface INotificationManager
    {
        Task<List<NotificationModel>> GetAll();
        Task<NotificationModel> GetById(int id);
        Task<NotificationModel> Create(NotificationModel notification);
        Task<NotificationModel> Update(NotificationModel notification);
        Task Delete(int id);
    }
}
