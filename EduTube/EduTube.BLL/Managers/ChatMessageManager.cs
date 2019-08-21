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

namespace EduTube.BLL.Managers
{
   public class ChatMessageManager : IChatMessageManager
   {
      private ApplicationDbContext _context;

      public ChatMessageManager(ApplicationDbContext context)
      {
         _context = context;
      }

      public async Task<List<ChatMessageModel>> GetAll()
      {
         return ChatMessageMapper.EntitiesToModels(await _context.ChatMessages.Where(x => !x.Deleted).ToListAsync());
      }

      public async Task<ChatMessageModel> GetById(int id, bool includeAll)
      {
         return includeAll ? ChatMessageMapper.EntityToModel(await _context.ChatMessages
             .Include(x => x.Chat).Include(x => x.Message).FirstAsync(x => x.Id == id && !x.Deleted))
             : ChatMessageMapper.EntityToModel(await _context.ChatMessages.FirstAsync(x => x.Id == id && !x.Deleted));
      }

      public async Task<ChatMessageModel> Create(ChatMessageModel chatMessage)
      {
         ChatMessage entity = ChatMessageMapper.ModelToEntity(chatMessage);
         _context.ChatMessages.Add(entity);
         await _context.SaveChangesAsync();
         return ChatMessageMapper.EntityToModel(entity);
      }

      public async Task<ChatMessageModel> Update(ChatMessageModel chatMessage)
      {
         _context.Update(ChatMessageMapper.ModelToEntity(chatMessage));
         await _context.SaveChangesAsync();
         return chatMessage;
      }

      public async Task Delete(int id)
      {
         ChatMessage entity = await _context.ChatMessages.FirstOrDefaultAsync(x => x.Id == id);
         _context.ChatMessages.Remove(entity);
         await _context.SaveChangesAsync();
      }

      public async Task DeleteActivateByUser(string id, bool option)
      {
         List<ChatMessage> chatMessages = await _context.ChatMessages.Where(x => x.UserId.Equals(id) && x.Deleted == !option).ToListAsync();
         chatMessages.Select(x => x.Deleted = option);
         _context.UpdateRange(chatMessages);
         await _context.SaveChangesAsync();
      }
   }
}
