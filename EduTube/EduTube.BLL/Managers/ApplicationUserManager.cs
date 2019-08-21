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

namespace EduTube.BLL.Managers
{
   public class ApplicationUserManager : IApplicationUserManager
   {
      private ApplicationDbContext _context;
      private UserManager<ApplicationUser> _userManager;
      private SignInManager<ApplicationUser> _signInManager;
      private IChatMessageManager _chatMessageManager;
      private IReactionManager _reactionManager;
      private ISubscriptionManager _subscriptionManager;
      private IVideoManager _videoManager;

      public ApplicationUserManager(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
         SignInManager<ApplicationUser> signInManager, IChatMessageManager chatMessageManager,
         IReactionManager reactionManager, ISubscriptionManager subscriptionManager,
         IVideoManager videoManager)
      {
         _context = context;
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

      public async Task<ApplicationUserModel> GetById(string id, bool includeAll)
      {
         return  includeAll ? UserMapper.EntityToModel(await _userManager.Users
               .Include(x => x.Videos).Include(x => x.Subscribers).Include(x => x.SubscribedOn)
               .Include(x => x.Notifications).FirstOrDefaultAsync(x => x.Id == id && !x.Deleted))
            : UserMapper.EntityToModel(await _userManager.Users.FirstOrDefaultAsync(x => x.Id == id && !x.Deleted));

         /*return UserMapper.EntityToModel(await _context.Users
             .Include(x => x.Videos).Include(x => x.Subscribers).Include(x => x.SubscribedOn)
             .Include(x => x.Notifications).FirstOrDefaultAsync(x => x.Id == id && !x.Deleted));*/
      }

      public async Task<ApplicationUserModel> GetByChannelName(string channelname)
      {
         return UserMapper.EntityToModel(await _userManager.Users
               .Include(x => x.Subscribers).ThenInclude(x => x.Subscriber)
               .Include(x => x.SubscribedOn).ThenInclude(x => x.SubscribedOn)
               .Include(x => x.Videos)
               .FirstOrDefaultAsync(x => x.ChannelName.Equals(channelname) && !x.Deleted));
         /*return UserMapper.EntityToModel(await _context.Users
             .Include(x => x.Subscribers).ThenInclude(x => x.Subscriber)
             .Include(x => x.SubscribedOn).ThenInclude(x => x.SubscribedOn)
             .Include(x => x.Videos)
             .FirstOrDefaultAsync(x => x.ChannelName.Equals(channelname) && !x.Deleted));*/
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
         _chatMessageManager.DeleteActivateByUser(id, true);
         /*ApplicationUser user = _context.Users.FirstOrDefault(x => x.Id == id);
         List<ChatMessage> chatMessages = await _context.ChatMessages.Where(x => x.UserId == user.Id).ToListAsync();
         List<Reaction> reactions = await _context.Reactions.Where(x => x.UserId == user.Id).ToListAsync();
         List<Subscription> subscriptions = await _context.Subscriptions.Where(x => x.SubscribedOnId == user.Id || x.SubscriberId == user.Id).ToListAsync();
         List<Video> videos = await _context.Videos.Where(x => x.UserId == user.Id).ToListAsync();

         user.Deleted = true;
         chatMessages.Select(x => x.Deleted = true);
         reactions.Select(x => x.Deleted = true);
         subscriptions.Select(x => x.Deleted = true);
         videos.Select(x => x.Deleted = true);
         _context.Update(user);
         _context.UpdateRange(chatMessages);
         _context.UpdateRange(reactions);
         _context.UpdateRange(subscriptions);
         _context.UpdateRange(videos);

         await _context.SaveChangesAsync();*/
      }

      public async Task Activate(string id)
      {
         ApplicationUser user = _userManager.Users.FirstOrDefault(x => x.Id == id);
         user.Deleted = false;
         await _userManager.UpdateAsync(user);
         
         /*ApplicationUser user = _context.Users.FirstOrDefault(x => x.Id == id);
         List<ChatMessage> chatMessages = await _context.ChatMessages.Where(x => x.UserId == user.Id).ToListAsync();
         List<Reaction> reactions = await _context.Reactions.Where(x => x.UserId == user.Id).ToListAsync();
         List<Subscription> subscriptions = await _context.Subscriptions.Where(x => x.SubscribedOnId == user.Id || x.SubscriberId == user.Id).ToListAsync();
         List<Video> videos = await _context.Videos.Where(x => x.UserId == user.Id).ToListAsync();

         user.Deleted = false;
         chatMessages.Select(x => x.Deleted = false);
         reactions.Select(x => x.Deleted = false);
         subscriptions.Select(x => x.Deleted = false);
         videos.Select(x => x.Deleted = false);
         _context.Update(user);
         _context.UpdateRange(chatMessages);
         _context.UpdateRange(reactions);
         _context.UpdateRange(subscriptions);
         _context.UpdateRange(videos);
         
         await _context.SaveChangesAsync();*/
      }

      public async Task<SignInResult> Login(string email, string password, bool rememberMe)
      {
         return await _signInManager.PasswordSignInAsync(email, password, rememberMe, lockoutOnFailure: false);
      }
   }
}
