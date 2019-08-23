using EduTube.BLL.Models;
using EduTube.DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EduTube.BLL.Managers.Interfaces
{
   public interface IChatManager
   {
      Task<List<ChatModel>> GetAll();
      Task<Chat> GetByIdEntity(int id, bool includeAll);
      Task<ChatModel> GetById(int id, bool includeAll);
      Task<ChatModel> Create(ChatModel chatModel);
      Task<ChatModel> Update(ChatModel chatModel);
      Task Delete(int id);
   }
}
