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
   public class EmojiManager : IEmojiManager
   {
      private ApplicationDbContext _context;

      public EmojiManager(ApplicationDbContext context)
      {
         _context = context;
      }

      public async Task<List<EmojiModel>> GetAll()
      {
         return EmojiMapper.EntitiesToModels(await _context.Emojis.Where(x => !x.Deleted).ToListAsync());
      }

      public async Task<EmojiModel> GetById(int id)
      {
         return EmojiMapper.EntityToModel(await _context.Emojis
             .FirstOrDefaultAsync(x => x.Id == id && !x.Deleted));
      }
      public async Task<int?> GetEmojiId(int videoId, string userId)
      {
         /*Reaction reaction = await _context.Reactions.FirstOrDefaultAsync(x => x.VideoId == videoId && userId.Equals(userId) && !x.Deleted);
         return reaction?.Id;*/
         return await _context.Reactions.Where(x => x.VideoId == videoId && x.UserId.Equals(userId) && !x.Deleted).Select(x => x.EmojiId).FirstOrDefaultAsync();
      }
      public async Task<EmojiModel> Create(EmojiModel emoji)
      {
         Emoji entity = EmojiMapper.ModelToEntity(emoji);
         _context.Emojis.Add(entity);
         await _context.SaveChangesAsync();
         return EmojiMapper.EntityToModel(entity);
      }

      public async Task<Models.EmojiModel> Update(Models.EmojiModel emoticon)
      {
         _context.Update(EmojiMapper.ModelToEntity(emoticon));
         await _context.SaveChangesAsync();
         return emoticon;
      }

      public async Task Delete(int id)
      {
         Emoji entity = await _context.Emojis.FirstOrDefaultAsync(x => x.Id == id);
         /*entity.Deleted = true;
         _context.Update(entity);*/
         _context.Emojis.Remove(entity);
         await _context.SaveChangesAsync();
      }
   }
}
