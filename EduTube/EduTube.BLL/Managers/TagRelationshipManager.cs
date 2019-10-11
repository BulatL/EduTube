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
   public class TagRelationshipManager : ITagRelationshipManager
   {
      private ApplicationDbContext _context;

      public TagRelationshipManager(ApplicationDbContext context)
      {
         _context = context;
      }
      public async Task<List<TagRelationshipModel>> GetByChat(int chatId)
      {
         return TagRelationshipMapper.EntitiesToModels(await _context.TagRelationships.Where(x => x.ChatId == chatId).ToListAsync());
      }

      public async Task<List<TagRelationshipModel>> GetByVideoId(int id, bool includeTag)
      {
         return includeTag ? TagRelationshipMapper.EntitiesToModels(
             await _context.TagRelationships.Include(x => x.Tag).Where(x => x.VideoId == id).ToListAsync())
             : TagRelationshipMapper.EntitiesToModels(await _context.TagRelationships.Where(x => x.VideoId == id).ToListAsync());
      }

      public async Task<List<int?>> Get2MostPopularTagsIdByVideoId(List<int> videosId)
      {

         return await _context.TagRelationships
             .Where(x => videosId.Contains(int.Parse(x.VideoId.ToString())))
             .GroupBy(x => x.TagId)
             .OrderByDescending(g => g.Count())
             .Take(2)
             .Select(x => x.Key)
             .ToListAsync();
      }

      public async Task Remove(int id)
      {
         TagRelationship entity = await _context.TagRelationships.FirstOrDefaultAsync(x => x.Id == id);
         _context.TagRelationships.Remove(entity);
         await _context.SaveChangesAsync();
      }
   }
}
