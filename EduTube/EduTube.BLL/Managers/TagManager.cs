﻿using EduTube.BLL.Managers.Interfaces;
using EduTube.BLL.Models;
using EduTube.DAL.Data;
using EduTube.BLL.Mappers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduTube.DAL.Entities;
using System.Diagnostics;

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
         try
         {
            return await _context.TagRelationships
                .Where(x => videosId.Contains(x.VideoId.Value))
                .GroupBy(x => x.TagId)
                .OrderByDescending(g => g.Count())
                .Take(2)
                .Select(x => x.Key)
                .ToListAsync();
         }
         catch(Exception e)
         {
            Debug.WriteLine(e.StackTrace);
         }
         return new List<int?>();
      }
   }
}
