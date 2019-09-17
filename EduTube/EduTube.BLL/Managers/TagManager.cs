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
   public class TagManager : ITagManager
   {
      private ApplicationDbContext _context;

      public TagManager(ApplicationDbContext context)
      {
         _context = context;
      }

      public async Task<List<TagModel>> GetAll()
      {
         return TagMapper.EntitiesToModels(await _context.Tags.ToListAsync());
      }

      public async Task<List<TagModel>> GetByNames(string names)
      {
         List<Tag> tags = new List<Tag>();
         string[] tagNames = names.Split(',');
         foreach (var tagName in tagNames)
         {
            Tag tag = await _context.Tags.FirstOrDefaultAsync(x => x.Name.Equals(tagName));

            if(tag == null)
               tag = new Tag() { Id = 0, Name = tagName };

            tags.Add(tag);
         }
         return TagMapper.EntitiesToModels(tags);
      }

      public async Task<TagModel> GetById(int id)
      {
         return TagMapper.EntityToModel(await _context.Tags.FirstOrDefaultAsync(x => x.Id == id));
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


      public async Task<TagModel> Create(TagModel hashtag)
      {
         Tag entity = TagMapper.ModelToEntity(hashtag);
         _context.Tags.Add(entity);
         await _context.SaveChangesAsync();
         return TagMapper.EntityToModel(entity);
      }

      public async Task<TagModel> Update(TagModel hashtag)
      {
         _context.Update(TagMapper.ModelToEntity(hashtag));
         await _context.SaveChangesAsync();
         return hashtag;
      }

      public async Task Delete(int id)
      {
         Tag entity = await _context.Tags.FirstOrDefaultAsync(x => x.Id == id);
         _context.Tags.Remove(entity);
         await _context.SaveChangesAsync();
      }
   }
}
