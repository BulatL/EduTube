using System.Collections.Generic;
using System.Threading.Tasks;
using EduTube.BLL.Models;

namespace EduTube.BLL.Managers.Interfaces
{
   public interface ITagRelationshipManager
   {
      Task<List<TagRelationshipModel>> GetByVideoId(int id, bool includeTag);
      Task<List<TagRelationshipModel>> GetByChat(int chatId);
      Task<List<int?>> Get2MostPopularTagsIdByVideoId(List<int> videosId);
      Task Remove(int id);
   }
}
