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
    public class HashtagManager : IHashtagManager
    {
        private ApplicationDbContext _context;

        public HashtagManager(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<HashtagModel>> GetAll()
        {
            return HashtagMapper.EntitiesToModels(await _context.Hashtags.ToListAsync());
        }

        public async Task<HashtagModel> GetById(int id)
        {
            return HashtagMapper.EntityToModel(await _context.Hashtags.FirstOrDefaultAsync(x => x.Id == id));
        }

        public async Task<HashtagModel> Create(HashtagModel hashtag)
        {
            Hashtag entity = HashtagMapper.ModelToEntity(hashtag);
            _context.Hashtags.Add(entity);
            await _context.SaveChangesAsync();
            return HashtagMapper.EntityToModel(entity);
        }

        public async Task<HashtagModel> Update(HashtagModel hashtag)
        {
            _context.Update(HashtagMapper.ModelToEntity(hashtag));
            await _context.SaveChangesAsync();
            return hashtag;
        }

        public async Task Delete(int id)
        {
            Hashtag entity = await _context.Hashtags.FirstOrDefaultAsync(x => x.Id == id);
            _context.Hashtags.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
