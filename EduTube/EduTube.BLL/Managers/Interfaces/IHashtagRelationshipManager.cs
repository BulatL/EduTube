using System.Collections.Generic;
using System.Threading.Tasks;
using EduTube.BLL.Models;

namespace EduTube.BLL.Managers.Interfaces
{
    public interface IHashtagRelationshipManager
    {
        Task<List<HashtagRelationshipModel>> GetAll();
        Task<List<HashtagRelationshipModel>> GetByVideoId(int id, bool includeHashtag);
        Task<HashtagRelationshipModel> GetById(int id, bool includeAll);
        Task<List<int?>> Get2MostPopularHashtagsIdByVideoId(List<int> videosId);
        Task<HashtagRelationshipModel> Create(HashtagRelationshipModel hashtagRelationship);
        Task<HashtagRelationshipModel> Update(HashtagRelationshipModel hashtagRelationship);
        Task Delete(int id);
    }
}
