using EduTube.BLL.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EduTube.BLL.Managers.Interfaces
{
   public interface IApplicationUserManager
   {
      Task<List<ApplicationUserModel>> GetAll();
      Task<string> GetCurrentUserRole(string id);
      Task<ApplicationUserModel> GetById(string id, bool includeAll);
      Task<ApplicationUserModel> GetByChannelName(string channelName);
      Task<ApplicationUserModel> GetByEmail(string email);
      Task<ApplicationUserModel> GetByEmailAndPassword(string email, string password);
      Task<bool> ChannelNameExist(string channelName, string userId);
      Task<bool> EmailExist(string email, string userId);
      Task<bool> Login(string email, string password, bool rememberMe);
      Task Logout();
      //ApplicationUserModel Create(int id);
      Task<ApplicationUserModel> Update(ApplicationUserModel userModel);
      Task Register(ApplicationUserModel user, string password);
      Task<IdentityResult> Delete(string id);
      Task Activate(string id);
      //Task SetNewClaims(ApplicationUserModel user);
   }
}
