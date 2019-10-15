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
using EduTube.DAL.Enums;
using EduTube.BLL.Enums;

namespace EduTube.BLL.Managers
{
   public class ApplicationUserManager : IApplicationUserManager
   {
      private readonly UserManager<ApplicationUser> _userManager;
      private readonly SignInManager<ApplicationUser> _signInManager;
      private readonly IChatMessageManager _chatMessageManager;
      private readonly IReactionManager _reactionManager;
      private readonly ISubscriptionManager _subscriptionManager;
      private readonly IVideoManager _videoManager;
      private readonly ApplicationDbContext _context;

      public ApplicationUserManager(UserManager<ApplicationUser> userManager,
         SignInManager<ApplicationUser> signInManager, IChatMessageManager chatMessageManager,
         IReactionManager reactionManager, ISubscriptionManager subscriptionManager,
         IVideoManager videoManager, ApplicationDbContext contex)
      {
         _userManager = userManager;
         _signInManager = signInManager;
         _chatMessageManager = chatMessageManager;
         _reactionManager = reactionManager;
         _subscriptionManager = subscriptionManager;
         _videoManager = videoManager;
         _context = contex;
      }

      public async Task<bool> Exist(string id)
      {
         return await _userManager.Users.AnyAsync(x => x.Id.Equals(id) && !x.Deleted);
      }

      public async Task<string> GetCurrentUserRole(string id)
      {
         ApplicationUser user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id.Equals(id));
         if (user == null)
            return "";
         
         IList<string> roles = await _userManager.GetRolesAsync(user);
         return roles[0];
      }

      public async Task<List<ApplicationUserModel>> GetAll()
      {
         return UserMapper.EntitiesToModels(await _userManager.Users.Where(x => !x.Deleted).ToListAsync());
      }

      public async Task<List<ApplicationUser>> GetAll(int take, int skip)
      {
         return await _userManager.Users.Skip(skip).Take(take).Where(x => !x.Deleted).ToListAsync();
      }

      public async Task<string> GetRole(ApplicationUser user)
      {
         IList<string> roles = await _userManager.GetRolesAsync(user);
         return roles.ElementAtOrDefault(0);
      }

      public async Task<string> GetRole(string userId)
      {
         ApplicationUser user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id.Equals(userId) && !x.Deleted && !x.Blocked);
         IList<string> roles = await _userManager.GetRolesAsync(user);
         return roles.ElementAtOrDefault(0);
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

      public async Task<ApplicationUserModel> GetByChannelName(string channelname, string userId, string userRole)
      {
         ApplicationUserModel user = UserMapper.EntityToModel(await _userManager.Users.FirstOrDefaultAsync(x => x.ChannelName.Equals(channelname) && !x.Deleted));
         if(user != null)
         {
            user.Subscribers = SubscriptionMapper.EntitiesToModels(await _context.Subscriptions.Include(x => x.Subscriber).Where(x => x.SubscribedOnId.Equals(user.Id) && !x.Deleted).ToListAsync());
            user.SubscribedOn = SubscriptionMapper.EntitiesToModels(await _context.Subscriptions.Include(x => x.SubscribedOn).Where(x => x.SubscriberId.Equals(user.Id) && !x.Deleted).ToListAsync());
            if (userRole.Equals("ADMIN"))
            {
               user.Videos = VideoMapper.EntitiesToModels(await _context.Videos.Where(x => x.UserId.Equals(user.Id) && !x.Deleted).ToListAsync());
            }
            else
            {
               if(userId == null)
               {
                  user.Videos = VideoMapper.EntitiesToModels(await _context.Videos.Where(x => x.UserId.Equals(user.Id) && !x.Deleted 
                  && x.VideoVisibility == VideoVisibility.Public).ToListAsync());
               }
               else
               {
                  user.Videos = VideoMapper.EntitiesToModels(await _context.Videos
                     .Where(x => x.UserId.Equals(user.Id) && !x.Deleted && (x.VideoVisibility != VideoVisibility.Invitation || x.UserId.Equals(userId))).ToListAsync());
               }
            }
         }
         return user;
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
            IdentityResult removeClaims = IdentityResult.Success;
            if (claims.Count > 0)
            {
               List<Claim> oldClaims = new List<Claim>();
               Claim oldProfileImage = claims.FirstOrDefault(x => x.Type.Equals("profileImage"));
               Claim oldChannelName = claims.FirstOrDefault(x => x.Type.Equals("channelName"));
               oldClaims.Add(oldProfileImage);
               oldClaims.Add(oldChannelName);
               removeClaims = await _userManager.RemoveClaimsAsync(entity, oldClaims);
            }
            if (removeClaims.Succeeded && claims.Count > 0)
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
         if (user == null)
            return IdentityResult.Failed();

         user.Deleted = true;
         List<ChatMessage> chatMessages = _context.ChatMessages.Where(x => x.UserId.Equals(id) && !x.Deleted).ToList();
         List<Subscription> subscriptions = _context.Subscriptions.Where(x => (x.SubscribedOnId.Equals(id) || x.SubscriberId.Equals(id)) && !x.Deleted).ToList();
         List<Video> videos = _context.Videos.Where(x => x.UserId.Equals(id) && !x.Deleted).ToList();
         List<int> videosId = videos.Select(v => v.Id).ToList();
         List<Comment> comments = _context.Comments.Where(x => (x.UserId.Equals(id) || videosId.Contains(x.VideoId)) && !x.Deleted).ToList();
         List<int> commentsId = comments.Select(v => v.Id).ToList();
         List<Reaction> reactions = _context.Reactions.Where(x => (x.UserId.Equals(id) || videosId.Contains(x.VideoId.GetValueOrDefault())) || commentsId.Contains(x.CommentId.GetValueOrDefault()) && !x.Deleted).ToList();

         chatMessages.ForEach(x => x.Deleted = true);
         subscriptions.ForEach(x => x.Deleted = true);
         comments.ForEach(x => x.Deleted = true);
         reactions.ForEach(x => x.Deleted = true);

         _context.ChatMessages.UpdateRange(chatMessages);
         _context.Subscriptions.UpdateRange(subscriptions);
         _context.Comments.UpdateRange(comments);
         _context.Reactions.UpdateRange(reactions);
         _context.Update(user);

         await _context.SaveChangesAsync();
         await _userManager.UpdateSecurityStampAsync(user);
         return IdentityResult.Success;
         /*await _chatMessageManager.DeleteActivateByUser(id, true);
         await _reactionManager.DeleteActivateByUser(id, true);
         await _subscriptionManager.DeleteActivateByUser(id, true);
         await _videoManager.DeleteActivateByUser(id, true);
         return await _userManager.UpdateAsync(user);*/
      }

      public async Task Activate(string id)
      {
         ApplicationUser user = _userManager.Users.FirstOrDefault(x => x.Id == id);
         if (user == null)
            return;

         user.Deleted = true;
         List<ChatMessage> chatMessages = _context.ChatMessages.Where(x => x.UserId.Equals(id) && x.Deleted).ToList();
         List<Subscription> subscriptions = _context.Subscriptions.Where(x => (x.SubscribedOnId.Equals(id) || x.SubscriberId.Equals(id)) && x.Deleted).ToList();
         List<Video> videos = _context.Videos.Where(x => x.UserId.Equals(id) && x.Deleted).ToList();
         List<int> videosId = videos.Select(v => v.Id).ToList();
         List<Comment> comments = _context.Comments.Where(x => (x.UserId.Equals(id) || videosId.Contains(x.VideoId)) && x.Deleted).ToList();
         List<int> commentsId = comments.Select(v => v.Id).ToList();
         List<Reaction> reactions = _context.Reactions.Where(x => (x.UserId.Equals(id) || videosId.Contains(x.VideoId.GetValueOrDefault())) || commentsId.Contains(x.CommentId.GetValueOrDefault()) && x.Deleted).ToList();

         chatMessages.ForEach(x => x.Deleted = false);
         subscriptions.ForEach(x => x.Deleted = false);
         comments.ForEach(x => x.Deleted = false);
         reactions.ForEach(x => x.Deleted = false);

         await _context.ChatMessages.AddRangeAsync(chatMessages);
         await _context.Subscriptions.AddRangeAsync(subscriptions);
         await _context.Comments.AddRangeAsync(comments);
         await _context.Reactions.AddRangeAsync(reactions);
         _context.Update(user);

         await _context.SaveChangesAsync();
         return;
         /*ApplicationUser user = _userManager.Users.FirstOrDefault(x => x.Id == id);
         user.Deleted = false;
         await _userManager.UpdateAsync(user);
         await _chatMessageManager.DeleteActivateByUser(id, false);
         await _reactionManager.DeleteActivateByUser(id, false);
         await _subscriptionManager.DeleteActivateByUser(id, false);
         await _videoManager.DeleteActivateByUser(id, false);*/
      }

      public async Task<bool> IsUserBlocked(string id)
      {
         ApplicationUser user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id.Equals(id));
         return user.Blocked;
      }

      public async Task<LoginResult> Login(string email, string password, bool rememberMe)
      {
         //return await _signInManager.PasswordSignInAsync(email, password, rememberMe, lockoutOnFailure: false);

         ApplicationUser user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email.Equals(email) && !x.Deleted);
         if (user == null)
            return LoginResult.UserNotFound;

         IPasswordHasher<ApplicationUser> passwordHasher = _userManager.PasswordHasher;
         PasswordVerificationResult verificationResult = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
         if (verificationResult == PasswordVerificationResult.Success)
			{
            if (user.Blocked)
            {
               return LoginResult.UserBlocked;
            }

				IList<Claim> claimsExist = await _userManager.GetClaimsAsync(user);
				if(claimsExist == null || claimsExist?.Count() == 0)
				{
					IList<string> roles = await _userManager.GetRolesAsync(user);
					List<Claim> claims = new List<Claim>();
					Claim id = new Claim("id", user.Id);
					Claim profileImage = new Claim("profileImage", user.ProfileImage);
					Claim channelName = new Claim("channelName", user.ChannelName.Replace(" ", "-"));
					Claim role = new Claim("role", roles[0]);
					claims.Add(id);
					claims.Add(profileImage);
					claims.Add(channelName);
					claims.Add(role);
					IdentityResult identity = await _userManager.AddClaimsAsync(user, claims);
					if (identity.Succeeded)
					{
						await _signInManager.SignInAsync(user, true);
						return LoginResult.Success;
					}
					else
					{
						return LoginResult.ClaimsFailed;
					}
				}
				else
				{
					await _signInManager.SignInAsync(user, true);
					return LoginResult.Success;
				}
         }
         return LoginResult.WrongCredentials;
      }

      public async Task Logout()
      {
         await _signInManager.SignOutAsync();
      }

      public async Task<IEnumerable<IdentityError>> Register(ApplicationUserModel model, string password)
      {
         ApplicationUser entity = UserMapper.ModelToEntity(model);
         IdentityResult result = await _userManager.CreateAsync(entity);
         if (result.Succeeded)
         {
            IdentityResult passwordResult = await _userManager.AddPasswordAsync(entity, password);
            if(!passwordResult.Succeeded)
            {
               return passwordResult.Errors;
            }
            await _userManager.AddToRoleAsync(entity, "User");
            return result.Errors;
         }
         return result.Errors;
      }

      public async Task<bool> SetClaims(ApplicationUser user)
      {
         IList<string> roles = await _userManager.GetRolesAsync(user);
         List<Claim> claims = new List<Claim>();
         Claim id = new Claim("id", user.Id);
         Claim profileImage = new Claim("profileImage", user.ProfileImage);
         Claim channelName = new Claim("channelName", user.ChannelName.Replace(" ", "-"));
         Claim role = new Claim("role", roles[0]);
         claims.Add(id);
         claims.Add(profileImage);
         claims.Add(channelName);
         claims.Add(role);
         IdentityResult identity = await _userManager.AddClaimsAsync(user, claims);
         if (identity.Succeeded)
            return true;
         else
            return false;
      }

      public async Task BlockUnblock(string id, bool blocked)
      {
         ApplicationUser user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id.Equals(id));
         user.Blocked = blocked;
         await _userManager.UpdateAsync(user);
         await _userManager.UpdateSecurityStampAsync(user);
      }

      public async Task PromoteDemote(string id, bool promote)
      {
         ApplicationUser user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id.Equals(id));
         if (promote)
         {
            await _userManager.RemoveFromRoleAsync(user, "User");
            await _userManager.AddToRoleAsync(user, "Admin");
         }
         else
         {
            await _userManager.RemoveFromRoleAsync(user, "Admin");
            await _userManager.AddToRoleAsync(user, "User");
         }

         IList<Claim> claimsExist = await _userManager.GetClaimsAsync(user);
         if (claimsExist != null && claimsExist?.Count() > 0)
         {
            IList<Claim> oldClaims = await _userManager.GetClaimsAsync(user);
            Claim oldRoleClaim = oldClaims.FirstOrDefault(x => x.Type.Equals("role"));
            IdentityResult removeClaims = await _userManager.RemoveClaimAsync(user, oldRoleClaim);

            if (promote)
            {
               Claim roleClaim = new Claim("role", "Admin");
               IdentityResult identity = await _userManager.AddClaimAsync(user, roleClaim);
            }
            else
            {
               Claim roleClaim = new Claim("role", "User");
               IdentityResult identity = await _userManager.AddClaimAsync(user, roleClaim);
            }
            await _userManager.UpdateSecurityStampAsync(user);
         }
      }

      public async Task<IdentityResult> ChangePassword(string userId, string oldPassowrd, string newPassword)
      {
         ApplicationUser user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id.Equals(userId) && !x.Deleted);
         if (user == null)
            return IdentityResult.Failed();

         return await _userManager.ChangePasswordAsync(user, oldPassowrd, newPassword);
         
      }
   }
}
