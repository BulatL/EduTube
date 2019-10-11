using EduTube.BLL.Models;
using System.Threading.Tasks;

namespace EduTube.BLL.Managers.Interfaces
{
   public interface IViewManager
   {
      Task<bool> ViewExist(int videoId, string userId, string ipAddress);
      Task<int> CountViewsByVideo(int videoId);
      Task<ViewModel> Create(int videoId, string userId, string ipAddress);
   }
}
