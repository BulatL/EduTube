﻿using System.Collections.Generic;
using System.Threading.Tasks;
using EduTube.BLL.Models;

namespace EduTube.BLL.Managers.Interfaces
{
   public interface IVideoManager
   {
      Task<List<VideoModel>> GetAll();
      Task<List<VideoModel>> GetTop5Videos(string userId);
      Task<List<int>> GetVideosIdByView(string userId, string ipAddress);
      Task<List<VideoModel>> Get6VideosByTag(string userId, int? tagId);
      Task<bool> CheckInvitationCode(int videoId, string invitationCode);
      Task<VideoModel> GetById(int id, bool includeAll);
      Task<VideoModel> Create(VideoModel video, string tagNames);
      Task<VideoModel> Update(VideoModel video, string tagNames);
      Task<int> Remove(int id);
   }
}
