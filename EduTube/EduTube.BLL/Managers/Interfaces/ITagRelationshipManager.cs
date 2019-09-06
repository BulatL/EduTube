using System.Collections.Generic;
using System.Threading.Tasks;
using EduTube.BLL.Models;

namespace EduTube.BLL.Managers.Interfaces
{
    public interface ITagRelationshipManager
    {
        Task<List<TagRelationshipModel>> GetAll();
        Task<List<TagRelationshipModel>> GetByVideoId(int id, bool includeHashtag);
        Task<TagRelationshipModel> GetById(int id, bool includeAll);
        Task<List<int?>> Get2MostPopularTagsIdByVideoId(List<int> videosId);
        Task<TagRelationshipModel> Create(TagRelationshipModel tagRelationship);
        Task<TagRelationshipModel> Update(TagRelationshipModel tagRelationship);
        Task Delete(int id);
    }
}
