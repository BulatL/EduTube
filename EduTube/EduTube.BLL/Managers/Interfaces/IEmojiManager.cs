using System.Collections.Generic;
using System.Threading.Tasks;
using EduTube.BLL.Models;

namespace EduTube.BLL.Managers.Interfaces
{
   public interface IEmojiManager
   {
		Task<List<EmojiModel>> GetAll();
      Task<EmojiModel> GetById(int id);
		Task Create(EmojiModel model);
		Task Update(EmojiModel model);
		Task Remove(int id);
		Task<bool> Exist(int id);
		Task<int?> GetEmojiId(int videoId, string userId);
	}
}
