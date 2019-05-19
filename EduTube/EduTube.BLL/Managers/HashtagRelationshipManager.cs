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
    public class HashtagRelationshipManager : IHashtagRelationshipManager
    {
        private ApplicationDbContext _context;

        public HashtagRelationshipManager(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<HashtagRelationshipModel>> GetAll()
        {
            return HashtagRelationshipMapper.EntitiesToModels(await _context.HashTagRelationships.ToListAsync());
        }

        public async Task<HashtagRelationshipModel> GetById(int id, bool includeAll)
        {
            return includeAll ? HashtagRelationshipMapper.EntityToModel(await _context.HashTagRelationships
                .Include(x => x.Chat).Include(x => x.Hashtag).Include(x => x.Video)
                .FirstOrDefaultAsync(x => x.Id == id))
                : HashtagRelationshipMapper.EntityToModel(await _context.HashTagRelationships
                .FirstOrDefaultAsync(x => x.Id == id));
        }

        public async Task<HashtagRelationshipModel> Create(HashtagRelationshipModel hashtagRelationship)
        {
            HashTagRelationship entity = HashtagRelationshipMapper.ModelToEntity(hashtagRelationship);
            _context.HashTagRelationships.Add(entity);
            await _context.SaveChangesAsync();
            return HashtagRelationshipMapper.EntityToModel(entity);
        }

        public async Task<HashtagRelationshipModel> Update(HashtagRelationshipModel hashtagRelationship)
        {
            _context.Update(HashtagRelationshipMapper.ModelToEntity(hashtagRelationship));
            await _context.SaveChangesAsync();
            return hashtagRelationship;
        }

        public async Task Delete(int id)
        {
            HashTagRelationship entity = await _context.HashTagRelationships.FirstOrDefaultAsync(x => x.Id == id);
            _context.HashTagRelationships.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
