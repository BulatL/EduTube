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
      public async Task<string> GetCurrentUserRole(string id)
      {
         ApplicationUser user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id.Equals(id));
         IList<string> roles = await _userManager.GetRolesAsync(user);
         return roles[0];
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

      public async Task<ApplicationUserModel> Update(ApplicationUserModel model)
      {
         bool refreshClaims = false;
         ApplicationUser entity = await _userManager.Users.FirstOrDefaultAsync(x => x.Id.Equals(model.Id) && !x.Deleted);
         if (!entity.ChannelName.Equals(model.ChannelName) || !entity.ProfileImage.Equals(model.ProfileImage))
            refreshClaims = true;

         UserMapper.CopyModelToEntity(model, entity);
         await _userManager.UpdateAsync(entity);

         if (refreshClaims)
         {
            IList<Claim> claims = await _userManager.GetClaimsAsync(entity);
            List<Claim> oldClaims = new List<Claim>();
            Claim oldProfileImage = claims.FirstOrDefault(x => x.Type.Equals("profileImage"));
            Claim oldChannelName = claims.FirstOrDefault(x => x.Type.Equals("channelName"));
            oldClaims.Add(oldProfileImage);
            oldClaims.Add(oldChannelName);
            IdentityResult removeClaims = await _userManager.RemoveClaimsAsync(entity, oldClaims);
            if (removeClaims.Succeeded)
            {
               List<Claim> newClaims = new List<Claim>();
               Claim profileImage = new Claim("profileImage", entity.ProfileImage);
               Claim channelName = new Claim("channelName", entity.ChannelName.Replace(" ", "-"));
               newClaims.Add(profileImage);
               newClaims.Add(channelName);
               await _userManager.AddClaimsAsync(entity, newClaims);
               await _signInManager.RefreshSignInAsync(entity);
            }
         }
         return UserMapper.EntityToModel(entity);
      }

      public async Task<IdentityResult> Delete(string id)
      {
         ApplicationUser user = _userManager.Users.FirstOrDefault(x => x.Id == id);
         user.Deleted = true;
         await _chatMessageManager.DeleteActivateByUser(id, true);
         await _reactionManager.DeleteActivateByUser(id, true);
         await _subscriptionManager.DeleteActivateByUser(id, true);
         await _videoManager.DeleteActivateByUser(id, true);
         return await _userManager.UpdateAsync(user);
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
            return false;
         }
         return false;
      }

      public async Task Logout()
      {
         await _signInManager.SignOutAsync();
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

      public async Task<bool> SetClaims(ApplicationUser user)
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
            return true;
         else
            return false;
      }
   }
}
