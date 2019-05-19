using System.Collections.Generic;
using System.Threading.Tasks;
using EduTube.BLL.Models;

namespace EduTube.BLL.Managers.Interfaces
{
    public interface IHashtagRelationshipManager
    {
        Task<List<HashtagRelationshipModel>> GetAll();
        Task<HashtagRelationshipModel> GetById(int id, bool includeAll);
        Task<HashtagRelationshipModel> Create(HashtagRelationshipModel hashtagRelationship);
        Task<HashtagRelationshipModel> Update(HashtagRelationshipModel hashtagRelationship);
        Task Delete(int id);
    }
}
