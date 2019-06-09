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
using EduTube.DAL.Enums;
using System.Diagnostics;

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

        public async Task<List<VideoModel>> GetTop5Videos(string userId)
        {
            if (userId != null)
            {
                return VideoMapper.EntitiesToModels(await _context.Videos
                    .Include(x => x.Views)
                    .Where(x => !x.Deleted && (x.VideoVisibility != VideoVisibility.Invitation || 
                    (x.VideoVisibility == VideoVisibility.Invitation && x.UserId == userId)))
                    .OrderByDescending(x => x.Views.Count())
                    .Take(5).ToListAsync());
            }
            else
            {
                return VideoMapper.EntitiesToModels(await _context.Videos
                   .Include(x => x.Views)
                   .Where(x => !x.Deleted && x.VideoVisibility == VideoVisibility.Public)
                   .OrderByDescending(x => x.Views.Count())
                   .Take(5).ToListAsync());
            }
        }

        public async Task<List<VideoModel>> SearchVideos(string userId, string search)
        {
            if (userId != null)
            {
                return VideoMapper.EntitiesToModels(await _context.Videos
                    .Include(x => x.Views)
                    .Where(x => !x.Deleted && (x.VideoVisibility != VideoVisibility.Invitation ||
                    (x.VideoVisibility == VideoVisibility.Invitation && x.UserId == userId)) &&
                    x.Name.Contains(search))
                    .OrderByDescending(x => x.Views.Count())
                    .Take(5).ToListAsync());
            }
            else
            {
                return VideoMapper.EntitiesToModels(await _context.Videos
                   .Include(x => x.Views)
                   .Where(x => !x.Deleted && x.VideoVisibility == VideoVisibility.Public &&
                    x.Name.Contains(search))
                   .OrderByDescending(x => x.Views.Count())
                   .Take(5).ToListAsync());
            }
        }

        public async Task<List<int>> GetVideosIdByView(string userId, string ipAddress)
        {
            return userId != null ? await _context.Views.Where(x => x.UserId == userId)
                    .Select(x => x.VideoId).Distinct().ToListAsync()
                    : await _context.Views.Where(x => x.IpAddress == ipAddress)
                    .Select(x => x.VideoId).Distinct().ToListAsync();
        }

        public async Task<List<VideoModel>> Get6VideosByHashtag(string userId, int? hashtagId)
        {
            List<Video> videos = new List<Video>();
            if (userId != null)
            {
                videos.AddRange(await _context.HashTagRelationships
                    .Include(h => h.Video)
                    .Where(h => !h.Video.Deleted && (h.Video.VideoVisibility != VideoVisibility.Invitation ||
                    (h.Video.VideoVisibility == VideoVisibility.Invitation && h.Video.UserId == userId)) &&
                    h.HashTagId == hashtagId)
                    .Select(h => h.Video)
                    .Take(6)
                    .ToListAsync()
                );
                
            }
            else
            {
                videos.AddRange(await _context.HashTagRelationships
                    .Include(h => h.Video)
                    .Where(h => !h.Video.Deleted && h.Video.VideoVisibility == VideoVisibility.Public &&
                    h.HashTagId == hashtagId)
                    .Select(h => h.Video)
                    .Take(6)
                    .ToListAsync()
                );
            }
            return VideoMapper.EntitiesToModels(videos);
        }

        public async Task<List<VideoModel>> GetRecommendedVideos(string userId, string ipAddress)
        {
            List<Video> videos = new List<Video>();
            List<int> videosId = new List<int>();
            List<int?> hashtagsId = new List<int?>();

            Debug.WriteLine("usao u menadzera " + DateTime.Now);
            if (userId != null)
            {
                videosId = await _context.Views.Where(x => x.UserId == userId)
                    .Select(x => x.VideoId).Distinct().ToListAsync();

                Debug.WriteLine("nasao videoId " + DateTime.Now);
                hashtagsId = await _context.HashTagRelationships
                    .Where(x => videosId.Contains(int.Parse(x.VideoId.ToString())))
                    .GroupBy(x => x.HashTagId)
                    .OrderByDescending(g => g.Count())
                    .Take(2)
                    .Select(x => x.Key)
                    .ToListAsync();


                Debug.WriteLine("nasao hastagsId " + DateTime.Now);
                foreach (var id in hashtagsId)
                {
                    videos.AddRange(await _context.HashTagRelationships
                        .Include(h => h.Video)
                        .Where(h => !h.Video.Deleted && (h.Video.VideoVisibility != VideoVisibility.Invitation ||
                        (h.Video.VideoVisibility == VideoVisibility.Invitation && h.Video.UserId == userId)) &&
                        h.HashTagId == id)
                        .Select(h => h.Video)
                        .Take(6)
                        .ToListAsync()
                    );
                }

                Debug.WriteLine("zavrsio petlju " + DateTime.Now);
                return VideoMapper.EntitiesToModels(videos);
            }
            else
            {
                videosId = await _context.Views.Where(x => x.IpAddress == ipAddress)
                    .Select(x => x.VideoId).Distinct().ToListAsync();

                Debug.WriteLine("nasao videoId " + DateTime.Now);
                hashtagsId = await _context.HashTagRelationships
                    .Where(x => videosId.Contains(int.Parse(x.VideoId.ToString())))
                    .GroupBy(x => x.HashTagId)
                    .OrderByDescending(g => g.Count())
                    .Take(2)
                    .Select(x => x.Key)
                    .ToListAsync();


                Debug.WriteLine("nasao hastagsId " + DateTime.Now);
                foreach (var id in hashtagsId)
                {
                    videos.AddRange(await _context.HashTagRelationships
                        .Include(h => h.Video)
                        .Where(h => !h.Video.Deleted && h.Video.VideoVisibility == VideoVisibility.Public  &&
                        h.HashTagId == id)
                        .Select(h => h.Video)
                        .Take(6)
                        .ToListAsync()
                    );
                }
                return VideoMapper.EntitiesToModels(videos);
            }
        }

        public async Task<VideoModel> GetById(int id, bool includeAll)
        {
            return includeAll ? VideoMapper.EntityToModel(await _context.Videos
                .Include(x => x.HashtagRelationships).ThenInclude(x => x.Hashtag).Include(x => x.User)
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
