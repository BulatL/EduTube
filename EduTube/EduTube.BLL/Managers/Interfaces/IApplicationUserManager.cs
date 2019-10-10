using EduTube.BLL.Enums;
using EduTube.BLL.Models;
using EduTube.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EduTube.BLL.Managers.Interfaces
{
   public interface IApplicationUserManager
   {
      Task<bool> Exist(string id);
      Task<List<ApplicationUserModel>> GetAll();
      Task<List<ApplicationUser>> GetAll(int take, int skip);
      Task<string> GetRole(ApplicationUser user);
      Task<string> GetRole(string userId);
      Task<string> GetCurrentUserRole(string id);
      Task<ApplicationUserModel> GetById(string id, bool includeAll);
      Task<ApplicationUserModel> GetByChannelName(string channelName, string userId, string userRole);
      Task<bool> ChannelNameExist(string channelName, string userId);
      Task<bool> EmailExist(string email, string userId);
      Task<bool> IsUserBlocked(string id);
      Task<LoginResult> Login(string email, string password, bool rememberMe);
      Task Logout();
      Task<ApplicationUserModel> Update(ApplicationUserModel userModel);
      Task<IEnumerable<IdentityError>> Register(ApplicationUserModel user, string password);
      Task<IdentityResult> Delete(string id);
      Task Activate(string id);
      Task BlockUnblock(string id, bool block);
      Task PromoteDemote(string id, bool promote);
      Task<IdentityResult> ChangePassword(string userId, string oldPassword, string newPassword);
   }
}
