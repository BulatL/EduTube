using System.Collections.Generic;
using System.Threading.Tasks;
using EduTube.BLL.Models;

namespace EduTube.BLL.Managers.Interfaces
{
    public interface IHashtagManager
    {
        Task<List<HashtagModel>> GetAll();
        Task<HashtagModel> GetById(int id);
        Task<HashtagModel> Create(HashtagModel hashtag);
        Task<HashtagModel> Update(HashtagModel hashtag);
        Task Delete(int id);
    }
}
