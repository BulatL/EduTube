using EduTube.BLL.Managers.Interfaces;
using EduTube.BLL.Mappers;
using EduTube.BLL.Models;
using EduTube.DAL.Data;
using EduTube.DAL.Entities;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace EduTube.BLL.Managers
{
   public class ChatManager : IChatManager
   {
      private readonly ApplicationDbContext _context;
      private readonly ITagManager _tagManager;
      private readonly ITagRelationshipManager _tagRelationshipManager;

      public ChatManager(ApplicationDbContext context, ITagManager tagManager, ITagRelationshipManager tagRelationshipManager)
      {
         _context = context;
         _tagManager = tagManager;
         _tagRelationshipManager = tagRelationshipManager;
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

      public async Task<ChatModel> Create(ChatModel chatModel, string tagNames)
      {
         List<TagModel> tags = await _tagManager.GetByNames(tagNames);
         foreach (var tag in tags)
         {
            TagRelationshipModel relationshipModel = new TagRelationshipModel() { Id = 0, Tag = tag, TagId = tag.Id, Chat = chatModel };
            chatModel.TagRelationships.Add(relationshipModel);
         }
         Chat chat = ChatMapper.ModelToEntity(chatModel);
         _context.Chats.Add(chat);
         await _context.SaveChangesAsync();
         return ChatMapper.EntityToModel(chat);

      }
      public async Task<ChatModel> Update(ChatModel model, string tagNames)
      {
         List<TagModel> tags = await _tagManager.GetByNames(tagNames);
         List<TagRelationshipModel> oldRelationships = await _tagRelationshipManager.GetByChat(model.Id);
         foreach (var tag in tags)
         {
            TagRelationshipModel oldRelationship = oldRelationships.FirstOrDefault(x => x.TagId == tag.Id);
            if (oldRelationship == null)
            {
               TagRelationshipModel relationshipModel = new TagRelationshipModel() { Id = 0, Tag = tag, TagId = tag.Id, Chat = model, ChatId = model.Id };
               model.TagRelationships.Add(relationshipModel);
            }
            else
            {
               oldRelationships.Remove(oldRelationship);
               model.TagRelationships.Add(oldRelationship);
            }
         }
         foreach (var item in oldRelationships)
         {
            await _tagRelationshipManager.Remove(item.Id);
         }
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
