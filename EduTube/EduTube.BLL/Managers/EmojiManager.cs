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

		public async Task Create(EmojiModel model)
		{
			Emoji entity = EmojiMapper.ModelToEntity(model);
			await _context.Emojis.AddAsync(entity);
			await _context.SaveChangesAsync();
		}

		public async Task<List<EmojiModel>> GetAll()
		{
			return EmojiMapper.EntitiesToModels(await _context.Emojis.Where(x => !x.Deleted).ToListAsync());
		}

		public async Task<EmojiModel> GetById(int id)
		{
			return EmojiMapper.EntityToModel(await _context.Emojis.FirstOrDefaultAsync(x => !x.Deleted && x.Id == id));
		}

		public async Task<int?> GetEmojiId(int videoId, string userId)
      {
         return await _context.Reactions.Where(x => x.VideoId == videoId && x.UserId.Equals(userId) && !x.Deleted).Select(x => x.EmojiId).FirstOrDefaultAsync();
      }

		public async Task Update(EmojiModel model)
		{
			Emoji entity = await _context.Emojis.FirstOrDefaultAsync(x => !x.Deleted && x.Id == model.Id);
			EmojiMapper.CopyModelToEntity(model, entity);
		   _context.Emojis.Update(entity);
			await _context.SaveChangesAsync();
		}

		public async Task Remove(int id)
		{
			Emoji entity = await _context.Emojis.FirstOrDefaultAsync(x => !x.Deleted && x.Id == id);
			List<Reaction> reactions = await _context.Reactions.Where(x => x.EmojiId == id && !x.Deleted).ToListAsync();
			_context.Reactions.RemoveRange(reactions);
			_context.Emojis.Remove(entity);
			await _context.SaveChangesAsync();
		}

		public async Task<bool> Exist(int id)
		{
			return await _context.Emojis.AnyAsync(x => x.Id == id && !x.Deleted);
		}
	}
}
