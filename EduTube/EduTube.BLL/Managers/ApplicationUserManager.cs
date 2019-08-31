using EduTube.BLL.Managers.Interfaces;
using EduTube.BLL.Models;
using EduTube.DAL.Data;
using EduTube.BLL.Mappers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduTube.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Security.Cryptography;

namespace EduTube.BLL.Managers
{
   public class ApplicationUserManager : IApplicationUserManager
   {
      private UserManager<ApplicationUser> _userManager;
      private SignInManager<ApplicationUser> _signInManager;
      private IChatMessageManager _chatMessageManager;
      private IReactionManager _reactionManager;
      private ISubscriptionManager _subscriptionManager;
      private IVideoManager _videoManager;

      public ApplicationUserManager(UserManager<ApplicationUser> userManager,
         SignInManager<ApplicationUser> signInManager, IChatMessageManager chatMessageManager,
         IReactionManager reactionManager, ISubscriptionManager subscriptionManager,
         IVideoManager videoManager)
      {
         _userManager = userManager;
         _signInManager = signInManager;
         _chatMessageManager = chatMessageManager;
         _reactionManager = reactionManager;
         _subscriptionManager = subscriptionManager;
         _videoManager = videoManager;
      }

      public async Task<List<ApplicationUserModel>> GetAll()
      {
         return UserMapper.EntitiesToModels(await _userManager.Users.Where(x => !x.Deleted).ToListAsync());
         //return UserMapper.EntitiesToModels(await _context.Users.Where(x => !x.Deleted).ToListAsync());
      }

      public async Task<ApplicationUserModel> GetByEmailAndPassword(string email, string password)
      {
         var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email.Equals(email) && !x.Deleted);
         IPasswordHasher<ApplicationUser> passwordHasher = _userManager.PasswordHasher;
         var b = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
         return new ApplicationUserModel();
      }


      public async Task<ApplicationUserModel> GetById(string id, bool includeAll)
      {
         return includeAll ? UserMapper.EntityToModel(await _userManager.Users
               .Include(x => x.Videos).Include(x => x.Subscribers).Include(x => x.SubscribedOn)
               .Include(x => x.Notifications).FirstOrDefaultAsync(x => x.Id == id && !x.Deleted))
            : UserMapper.EntityToModel(await _userManager.Users.FirstOrDefaultAsync(x => x.Id == id && !x.Deleted));

      }

      public async Task<bool> ChannelNameExist(string channelName, string userId)
      {
         return await _userManager.Users.AnyAsync(x => !x.Id.Equals(userId) && x.ChannelName.Equals(channelName));
      }
      public async Task<bool> EmailExist(string email, string userId)
      {
         return await _userManager.Users.AnyAsync(x => !x.Id.Equals(userId) && x.NormalizedEmail.Equals(email.ToUpper()));
      }

      public async Task<ApplicationUserModel> GetByChannelName(string channelname)
      {
         return UserMapper.EntityToModel(await _userManager.Users
               .Include(x => x.Subscribers).ThenInclude(x => x.Subscriber)
               .Include(x => x.SubscribedOn).ThenInclude(x => x.SubscribedOn)
               .Include(x => x.Videos)
               .FirstOrDefaultAsync(x => x.ChannelName.Equals(channelname) && !x.Deleted));
      }
      public async Task<ApplicationUserModel> GetByEmail(string email)
      {
         return UserMapper.EntityToModel(await _userManager.Users
               .FirstOrDefaultAsync(x => x.Email.Equals(email) && !x.Deleted));
      }

      public async Task<ApplicationUserModel> Update(ApplicationUserModel userModel)
      {
         await _userManager.UpdateAsync(UserMapper.ModelToEntity(userModel));
         /*_context.Update(UserMapper.ModelToEntity(userModel));
         await _context.SaveChangesAsync();*/
         return userModel;
      }

      public async Task Delete(string id)
      {
         ApplicationUser user = _userManager.Users.FirstOrDefault(x => x.Id == id);
         user.Deleted = true;
         await _userManager.UpdateAsync(user);
         await _chatMessageManager.DeleteActivateByUser(id, true);
         await _reactionManager.DeleteActivateByUser(id, true);
         await _subscriptionManager.DeleteActivateByUser(id, true);
         await _videoManager.DeleteActivateByUser(id, true);
      }

      public async Task Activate(string id)
      {
         ApplicationUser user = _userManager.Users.FirstOrDefault(x => x.Id == id);
         user.Deleted = false;
         await _userManager.UpdateAsync(user);
         await _chatMessageManager.DeleteActivateByUser(id, false);
         await _reactionManager.DeleteActivateByUser(id, false);
         await _subscriptionManager.DeleteActivateByUser(id, false);
         await _videoManager.DeleteActivateByUser(id, false);
      }

      public async Task<bool> Login(string email, string password, bool rememberMe)
      {
         //return await _signInManager.PasswordSignInAsync(email, password, rememberMe, lockoutOnFailure: false);

         ApplicationUser user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email.Equals(email) && !x.Deleted);
         IPasswordHasher<ApplicationUser> passwordHasher = _userManager.PasswordHasher;
         PasswordVerificationResult verificationResult = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
         if (verificationResult == PasswordVerificationResult.Success)
         {
            IList<string> roles = await _userManager.GetRolesAsync(user);
            List<Claim> claims = new List<Claim>();
            Claim profileImage = new Claim("profileImage", user.ProfileImage);
            Claim channelName = new Claim("channelName", user.ChannelName.Replace(" ", "-"));
            Claim role = new Claim("role", roles[0]);
            claims.Add(profileImage);
            claims.Add(channelName);
            claims.Add(role);
            IdentityResult identity = await _userManager.AddClaimsAsync(user, claims);
            if (identity.Succeeded)
            {
               await _signInManager.SignInAsync(user, true);
               return true;
            }
         }
         return false;
      }

      public async Task Register(ApplicationUserModel model, string password)
      {
         ApplicationUser entity = UserMapper.ModelToEntity(model);
         IdentityResult result = await _userManager.CreateAsync(entity);
         if (result.Succeeded)
         {
            await _userManager.AddPasswordAsync(entity, password);
            await _userManager.AddToRoleAsync(entity, "User");
         }
      }
   }
}
