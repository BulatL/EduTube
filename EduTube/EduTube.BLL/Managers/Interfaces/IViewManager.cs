using EduTube.BLL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EduTube.BLL.Managers.Interfaces
{
   public interface IViewManager
   {
      Task<List<ViewModel>> GetAll();
      Task<ViewModel> GetById(int id);
      Task<bool> ViewExist(int videoId, string userId, string ipAddress);
      Task<int> CountViewsByVideo(int videoId);
      Task<ViewModel> Create(int videoId, string userId, string ipAddress);
      Task<ViewModel> Update(ViewModel view);
      Task Delete(int id);
   }
}
