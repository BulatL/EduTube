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
   public class EmoticonManager : IEmoticonManager
   {
      private ApplicationDbContext _context;

      public EmoticonManager(ApplicationDbContext context)
      {
         _context = context;
      }

      public async Task<List<EmoticonModel>> GetAll()
      {
         return EmoticonMapper.EntitiesToModels(await _context.Emoticons.Where(x => !x.Deleted).ToListAsync());
      }

      public async Task<EmoticonModel> GetById(int id)
      {
         return EmoticonMapper.EntityToModel(await _context.Emoticons
             .FirstOrDefaultAsync(x => x.Id == id && !x.Deleted));
      }
      public async Task<int?> GetEmoticonId(int videoId, string userId)
      {
         return await _context.Reactions.Where(x => x.VideoId == videoId && x.UserId.Equals(userId) && !x.Deleted).Select(x => x.EmoticonId).FirstOrDefaultAsync();
      }
      public async Task<EmoticonModel> Create(EmoticonModel emoticon)
      {
         Emoticon entity = EmoticonMapper.ModelToEntity(emoticon);
         _context.Emoticons.Add(entity);
         await _context.SaveChangesAsync();
         return EmoticonMapper.EntityToModel(entity);
      }

      public async Task<EmoticonModel> Update(EmoticonModel emoticon)
      {
         _context.Update(EmoticonMapper.ModelToEntity(emoticon));
         await _context.SaveChangesAsync();
         return emoticon;
      }

      public async Task Delete(int id)
      {
         Emoticon entity = await _context.Emoticons.FirstOrDefaultAsync(x => x.Id == id);
         /*entity.Deleted = true;
         _context.Update(entity);*/
         _context.Emoticons.Remove(entity);
         await _context.SaveChangesAsync();
      }
   }
}
