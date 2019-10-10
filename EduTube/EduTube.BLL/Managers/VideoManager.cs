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
      private readonly ApplicationDbContext _context;
      private readonly INotificationManager _notificationManager;
      private readonly ITagManager _tagManager;
      private readonly ITagRelationshipManager _tagRelationshipManager;

      public VideoManager(ApplicationDbContext context, INotificationManager notificationManager,
         ITagManager tagManager, ITagRelationshipManager tagRelationshipManager)
      {
         _context = context;
         _notificationManager = notificationManager;
         _tagManager = tagManager;
         _tagRelationshipManager = tagRelationshipManager;
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
                .Include(x => x.User)
                .Where(x => !x.Deleted && (x.VideoVisibility != VideoVisibility.Invitation ||
                (x.VideoVisibility == VideoVisibility.Invitation && x.UserId == userId)))
                .OrderByDescending(x => x.Views.Count())
                .Take(5).ToListAsync());
         }
         else
         {
            return VideoMapper.EntitiesToModels(await _context.Videos
               .Include(x => x.Views)
                .Include(x => x.User)
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
         return await _context.Views.Where(x => x.UserId.Equals(userId) || x.IpAddress.Equals(ipAddress))
                 .Select(x => x.VideoId).Distinct().ToListAsync();
      }

      public async Task<List<VideoModel>> Get6VideosByTag(string userId, int? tagId)
      {
         List<Video> videos = new List<Video>();
         if (userId != null)
         {
            videos.AddRange(await _context.TagRelationships
                .Include(h => h.Video)
                .Where(h => !h.Video.Deleted && (h.Video.VideoVisibility != VideoVisibility.Invitation ||
                (h.Video.VideoVisibility == VideoVisibility.Invitation && h.Video.UserId == userId)) &&
                h.TagId == tagId)
                .Select(h => h.Video)
                .Include(h => h.User)
                .Take(6)
                .ToListAsync()
            );

         }
         else
         {
            videos.AddRange(await _context.TagRelationships
                .Include(h => h.Video)
                .Where(h => !h.Video.Deleted && h.Video.VideoVisibility == VideoVisibility.Public && h.TagId == tagId)
                .Select(h => h.Video)
                .Include(h => h.User)
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
            hashtagsId = await _context.TagRelationships
                .Where(x => videosId.Contains(int.Parse(x.VideoId.ToString())))
                .GroupBy(x => x.TagId)
                .OrderByDescending(g => g.Count())
                .Take(2)
                .Select(x => x.Key)
                .ToListAsync();


            Debug.WriteLine("nasao hastagsId " + DateTime.Now);
            foreach (var id in hashtagsId)
            {
               videos.AddRange(await _context.TagRelationships
                   .Include(h => h.Video)
                   .Where(h => !h.Video.Deleted && (h.Video.VideoVisibility != VideoVisibility.Invitation ||
                   (h.Video.VideoVisibility == VideoVisibility.Invitation && h.Video.UserId == userId)) &&
                   h.TagId == id)
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
            hashtagsId = await _context.TagRelationships
                .Where(x => videosId.Contains(int.Parse(x.VideoId.ToString())))
                .GroupBy(x => x.TagId)
                .OrderByDescending(g => g.Count())
                .Take(2)
                .Select(x => x.Key)
                .ToListAsync();


            Debug.WriteLine("nasao hastagsId " + DateTime.Now);
            foreach (var id in hashtagsId)
            {
               videos.AddRange(await _context.TagRelationships
                   .Include(h => h.Video)
                   .Where(h => !h.Video.Deleted && h.Video.VideoVisibility == VideoVisibility.Public &&
                   h.TagId == id)
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
         Video entity = new Video();
         if (includeAll)
         {
            entity = await _context.Videos
            .Include(x => x.TagRelationships).ThenInclude(x => x.Tag)
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.Id == id && !x.Deleted);
            if(entity != null)
            {
               entity.Reactions = await _context.Reactions.Where(x => x.VideoId == id && !x.Deleted).ToListAsync();
               entity.Comments = await _context.Comments.Include(x => x.User).Where(x => x.VideoId == id && !x.Deleted)
                  .OrderByDescending(x => x.DateCreatedOn).Take(5).ToListAsync();
               entity.Comments.ForEach(x => x.Reactions = _context.Reactions.Where(r => r.CommentId == x.Id && !r.Deleted).ToList());
            }
         }
         else
         {
            entity = await _context.Videos.FirstOrDefaultAsync(x => x.Id == id && !x.Deleted);
         }
         return VideoMapper.EntityToModel(entity);
      }

      public async Task<bool> CheckInvitationCode(int videoId, string invitationCode)
      {
         return await _context.Videos.AnyAsync(x => x.Id == videoId && x.InvitationCode.Equals(invitationCode));
      }

      public async Task<VideoModel> Create(VideoModel model, string tagNames)
      {
         List<TagModel> tags = await _tagManager.GetByNames(tagNames);
         foreach (var item in tags)
         {
            TagRelationshipModel tr = new TagRelationshipModel() { Id = 0, TagId = item.Id, Tag = item, Video = model };
            model.TagRelationships.Add(tr);
         }
         Video entity = VideoMapper.ModelToEntity(model);
         _context.Videos.Add(entity);
         await _context.SaveChangesAsync();
         await _notificationManager.CreateByVideo(model.UserChannelName, entity.UserId, model.User.ProfileImage, entity.Name, entity.Id, entity.DateCreatedOn);
         return VideoMapper.EntityToModel(entity);

      }

      public async Task<VideoModel> Update(VideoModel model, string tagNames)
      {
         List<TagModel> tags = await _tagManager.GetByNames(tagNames);
         List<TagRelationshipModel> oldRelationships = await _tagRelationshipManager.GetByVideoId(model.Id, false);
         foreach (var tag in tags)
         {
            TagRelationshipModel oldRelationship = oldRelationships.FirstOrDefault(x => x.TagId == tag.Id);
            if (oldRelationship == null)
            {
               TagRelationshipModel relationshipModel = new TagRelationshipModel() { Id = 0, Tag = tag, TagId = tag.Id, Video = model, VideoId = model.Id };
               model.TagRelationships.Add(relationshipModel);
            }
            else
            {
               oldRelationships.Remove(oldRelationship);
               model.TagRelationships.Add(oldRelationship);
            }
         }
         foreach (var item in oldRelationships)
         {
            await _tagRelationshipManager.Remove(item.Id);
         }
         Video entity = await _context.Videos.Include(x => x.User).FirstOrDefaultAsync(x => x.Id == model.Id && !x.Deleted);
         VideoMapper.CopyModelToEntity(model, entity);
         await _context.SaveChangesAsync();
         return VideoMapper.EntityToModel(entity);
      }

      public async Task<int> Delete(int id)
      {
         Video video = await _context.Videos.FirstOrDefaultAsync(x => x.Id == id && !x.Deleted);
         if (video == null)
            return 0;
         List<Comment> comments = await _context.Comments.Where(x => x.VideoId == id && !x.Deleted).ToListAsync();
         List<Reaction> reactions = await _context.Reactions.Where(x => x.VideoId == id && !x.Deleted).ToListAsync();

         video.Deleted = true;
         comments.ForEach(x => x.Deleted = true);
         reactions.ForEach(x => x.Deleted = true);
         _context.Update(video);
         _context.UpdateRange(comments);
         _context.UpdateRange(reactions);
         return await _context.SaveChangesAsync();
      }

      public async Task<int> Remove(int id)
      {
         Video entity = await _context.Videos.FirstOrDefaultAsync(x => x.Id == id && !x.Deleted);
         if (entity == null)
            return 0;
         List<Comment> comments = await _context.Comments.Where(x => x.VideoId == id && !x.Deleted).ToListAsync();
         List<Reaction> reactions = await _context.Reactions.Where(x => x.VideoId == id && !x.Deleted).ToListAsync();

         _context.Videos.Remove(entity);
         _context.Comments.RemoveRange(comments);
         _context.Reactions.RemoveRange(reactions);
         return await _context.SaveChangesAsync();
      }

      public async Task DeleteActivateByUser(string id, bool option)
      {
         List<Video> videos = await _context.Videos.Where(x => x.UserId.Equals(id) && x.Deleted == !option).ToListAsync();
         videos.Select(x => x.Deleted = option);
         _context.UpdateRange(videos);
         await _context.SaveChangesAsync();
      }
   }
}
