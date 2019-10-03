using EduTube.BLL.Managers.Interfaces;
using EduTube.BLL.Mappers;
using EduTube.BLL.Models;
using EduTube.DAL.Data;
using EduTube.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EduTube.BLL.Managers
{
   public class ChatManager : IChatManager
   {
      private ApplicationDbContext _context;

      public ChatManager(ApplicationDbContext context)
      {
         _context = context;
      }

      public async Task<List<ChatModel>> GetAll()
      {
         IEnumerable<Chat> chats = await _context.Chats.Include(x => x.TagRelationships).ThenInclude(x => x.Tag).Where(x => !x.Deleted).ToListAsync();
         return ChatMapper.EntitiesToModels(chats);
      }

      public async Task<ChatModel> GetById(int id)
      {
         return ChatMapper.EntityToModel(await _context.Chats
             .Include(x => x.TagRelationships).ThenInclude(x => x.Tag)
             .FirstAsync(x => x.Id == id && !x.Deleted));
      }

      public async Task<ChatModel> Create(ChatModel chatModel)
      {
         Chat chat = ChatMapper.ModelToEntity(chatModel);
         _context.Chats.Add(chat);
         await _context.SaveChangesAsync();
         return ChatMapper.EntityToModel(chat);

      }

      public async Task<ChatModel> Update(ChatModel model)
      {
         Chat entity = await _context.Chats.FirstOrDefaultAsync(x => x.Id == model.Id && !x.Deleted);
         ChatMapper.CopyModelToEntity(model, entity);
         await _context.SaveChangesAsync();
         return ChatMapper.EntityToModel(entity);
      }

      public async Task Delete(int id)
      {
         Chat chat = await _context.Chats.FirstOrDefaultAsync(x => x.Id == id && !x.Deleted);
         if (chat == null)
            return;
         List<ChatMessage> messages = await _context.ChatMessages.Where(x => x.ChatId == chat.Id && !x.Deleted).ToListAsync();
         chat.Deleted = true;
         messages.ForEach(x => x.Deleted = true);
         _context.UpdateRange(messages);
         _context.Update(chat);

         await _context.SaveChangesAsync();
      }
   }
}
