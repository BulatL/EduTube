using EduTube.BLL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EduTube.BLL.Managers.Interfaces
{
   public interface IChatMessageManager
   {
      Task<List<ChatMessageModel>> GetByChat(int id);
      Task<ChatMessageModel> Create(ChatMessageModel chatMessage);
   }
}
