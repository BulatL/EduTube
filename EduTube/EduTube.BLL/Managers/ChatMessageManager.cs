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

      public async Task<List<ChatMessageModel>> GetByChat(int id)
      {
         return ChatMessageMapper.EntitiesToModels(await _context.ChatMessages.Include(x => x.User).Where(x => x.ChatId == id && !x.Deleted).ToListAsync());
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
   }
}
