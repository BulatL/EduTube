using EduTube.BLL.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EduTube.BLL.Managers.Interfaces
{
   public interface IApplicationUserManager
   {
      Task<List<ApplicationUserModel>> GetAll();
      Task<ApplicationUserModel> GetById(string id, bool includeAll);
      Task<ApplicationUserModel> GetByChannelName(string channelName);
      Task<SignInResult> Login(string email, string password, bool rememberMe);
      //ApplicationUserModel Create(int id);
      Task<ApplicationUserModel> Update(ApplicationUserModel userModel);
      Task Delete(string id);
      Task Activate(string id);
   }
}
