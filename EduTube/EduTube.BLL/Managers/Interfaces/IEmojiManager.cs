using System.Collections.Generic;
using System.Threading.Tasks;
using EduTube.BLL.Models;

namespace EduTube.BLL.Managers.Interfaces
{
   public interface IEmojiManager
   {
      Task<int?> GetEmojiId(int videoId, string userId);
   }
}
