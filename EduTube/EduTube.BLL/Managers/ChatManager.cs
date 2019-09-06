﻿using EduTube.BLL.Managers.Interfaces;
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
         IEnumerable<Chat> chats = await _context.Chats.Where(x => !x.Deleted).ToListAsync();
         return ChatMapper.EntitiesToModels(chats);
      }

      public async Task<Chat> GetByIdEntity(int id, bool includeAll)
      {
         //Expression<Func<Chat,bool>> includeExpression
         return includeAll ? await _context.Chats
             .Include(x => x.Messages).Include(x => x.TagRelationspis).ThenInclude(x => x.Tag)
             .FirstAsync(x => x.Id == id && !x.Deleted)
             : await _context.Chats.FirstAsync(x => x.Id == id && !x.Deleted);
      }

      public async Task<ChatModel> GetById(int id, bool includeAll)
      {
         //Expression<Func<Chat,bool>> includeExpression
         return includeAll ? ChatMapper.EntityToModel(await _context.Chats
             .Include(x => x.Messages).Include(x => x.TagRelationspis).ThenInclude(x => x.Tag)
             .FirstAsync(x => x.Id == id && !x.Deleted))
             : ChatMapper.EntityToModel(await _context.Chats.FirstAsync(x => x.Id == id && !x.Deleted));
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
         /*_context.Attach(chatModel).State = EntityState.Modified;
         _context.SaveChanges();*/
         _context.Update(ChatMapper.ModelToEntity(model));
         /*Chat oldEntity = await GetByIdEntity(model.Id, true);
         ChatMapper.CopyModelToEntity(model, oldEntity);*/
         await _context.SaveChangesAsync();
         return model;
      }

      public async Task Delete(int id)
      {
         Chat chat = _context.Chats.FirstOrDefault(x => x.Id == id && !x.Deleted);
         /*chat.Deleted = true;
         _context.Update(chat);*/
         _context.Chats.Remove(chat);
         await _context.SaveChangesAsync();
      }
   }
}
