using System.Collections.Generic;
using System.Threading.Tasks;
using EduTube.BLL.Models;

namespace EduTube.BLL.Managers.Interfaces
{
   public interface IVideoManager
   {
      Task<List<VideoModel>> GetAll();
      Task<List<VideoModel>> GetTop5Videos(string userId);
      Task<List<VideoModel>> SearchVideos(string userId, string search);
      Task<List<int>> GetVideosIdByView(string userId, string ipAddress);
      Task<List<VideoModel>> Get6VideosByHashtag(string userId, int? hashtagId);
      Task<List<VideoModel>> GetRecommendedVideos(string userId, string ipAddress);
      Task<VideoModel> GetById(int id, bool includeAll);
      Task<VideoModel> Create(VideoModel video);
      Task<VideoModel> Update(VideoModel video);
      Task<int> Delete(int id);
      Task<int> Remove(int id);
      Task DeleteActivateByUser(string id, bool option);
   }
}
