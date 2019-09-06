using System.Collections.Generic;
using System.Threading.Tasks;
using EduTube.BLL.Models;

namespace EduTube.BLL.Managers.Interfaces
{
   public interface IEmojiManager
   {
      Task<List<EmojiModel>> GetAll();
      Task<EmojiModel> GetById(int id);
      Task<int?> GetEmojiId(int videoId, string userId);
      Task<EmojiModel> Create(EmojiModel emoji);
      Task<EmojiModel> Update(EmojiModel emoji);
      Task Delete(int id);
   }
}
