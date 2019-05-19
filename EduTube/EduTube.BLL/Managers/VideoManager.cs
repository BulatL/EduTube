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
    public class VideoManager : IVideoManager
    {
        private ApplicationDbContext _context;

        public VideoManager(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<VideoModel>> GetAll()
        {
            return VideoMapper.EntitiesToModels(await _context.Videos.Where(x => !x.Deleted).ToListAsync());
        }

        public async Task<VideoModel> GetById(int id, bool includeAll)
        {
            return includeAll ? VideoMapper.EntityToModel(await _context.Videos
                .Include(x => x.Hashtags).ThenInclude(x => x.Hashtag).Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == id && !x.Deleted))
                : VideoMapper.EntityToModel(await _context.Videos
                .FirstOrDefaultAsync(x => x.Id == id && !x.Deleted));
        }

        public async Task<VideoModel> Create(VideoModel model)
        {
            Video entity = VideoMapper.ModelToEntity(model);
            _context.Videos.Add(entity);
            await _context.SaveChangesAsync();
            return VideoMapper.EntityToModel(entity);

        }

        public async Task<VideoModel> Update(VideoModel model)
        {
            _context.Update(VideoMapper.ModelToEntity(model));
            await _context.SaveChangesAsync();
            return model;
        }

        public async Task Delete(int id)
        {
            Video entity = await _context.Videos.FirstOrDefaultAsync(x => x.Id == id);
            /*entity.Deleted = true;
            _context.Update(entity);*/
            _context.Videos.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
