using System.Collections.Generic;
using System.Threading.Tasks;
using EduTube.BLL.Models;

namespace EduTube.BLL.Managers.Interfaces
{
    public interface ITagManager
    {
        Task<List<TagModel>> GetAll();
        Task<TagModel> GetById(int id);
        Task<List<int?>> Get2MostPopularTagsIdByVideoId(List<int> videosId);
        Task<TagModel> Create(TagModel tag);
        Task<TagModel> Update(TagModel tag);
        Task Delete(int id);
    }
}
