using System.Collections.Generic;
using System.Threading.Tasks;
using EduTube.BLL.Models;

namespace EduTube.BLL.Managers.Interfaces
{
    public interface IVideoManager
    {
        Task<List<VideoModel>> GetAll();
        Task<VideoModel> GetById(int id, bool includeAll);
        Task<VideoModel> Create(VideoModel video);
        Task<VideoModel> Update(VideoModel video);
        Task Delete(int id);
    }
}
