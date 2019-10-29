using EduTube.BLL.Managers.Interfaces;
using EduTube.BLL.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EduTube.GUI.SignalRHubs
{
   public class ChatHub : Hub
   {
      private readonly IApplicationUserManager _userManager;
      private readonly IChatMessageManager _chatMessageManager;

      public ChatHub(IApplicationUserManager userManager, IChatMessageManager chatMessageManager)
      {
         _userManager = userManager;
         _chatMessageManager = chatMessageManager;
      }
      public async Task SendMessage(string message,int chatId, string chatName)
      {
         ApplicationUserModel currentUser = await _userManager.GetById(Context.User.Claims.FirstOrDefault(x => x.Type.Equals("id")).Value, false);
         await Clients.Group(chatName).SendAsync("ReceiveMessage", currentUser.Id, currentUser.ChannelName, currentUser.ProfileImage, message);

         ChatMessageModel messageModel = new ChatMessageModel()
         {
            ChatId = chatId,
            DateCreatedOn = DateTime.Now,
            Deleted = false,
            Message = message,
            UserId = currentUser.Id
         };
         await _chatMessageManager.Create(messageModel);
      }

      public async Task JoinRoom(string chatName)
      {
         await Groups.AddToGroupAsync(Context.ConnectionId, chatName);
         await Clients.Group(chatName).SendAsync("ReceiveMessage", Context.User.Claims.FirstOrDefault(x => x.Type.Equals("id")).Value, 
                                                                   Context.User.Claims.FirstOrDefault(x => x.Type.Equals("channelName")).Value, 
                                                                   Context.User.Claims.FirstOrDefault(x => x.Type.Equals("profileImage")).Value, "joined");
      }

      public Task LeaveRoom(string chatName)
      {
         return Groups.RemoveFromGroupAsync(Context.ConnectionId, chatName);
      }
   }
}
