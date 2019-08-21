using EduTube.BLL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EduTube.BLL.Managers.Interfaces
{
   public interface IChatMessageManager
   {
      Task<List<ChatMessageModel>> GetAll();
      Task<ChatMessageModel> GetById(int id, bool includeAll);
      Task<ChatMessageModel> Create(ChatMessageModel chatMessage);
      Task<ChatMessageModel> Update(ChatMessageModel chatMessage);
      Task Delete(int id);
      Task DeleteActivateByUser(string id, bool option);
   }
}
