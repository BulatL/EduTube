using EduTube.BLL.Models;
using EduTube.DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EduTube.BLL.Managers.Interfaces
{
   public interface IChatManager
   {
      Task<List<ChatModel>> GetAll();
      Task<ChatModel> GetById(int id);
      Task<ChatModel> Create(ChatModel chatModel, string tagNames);
      Task<ChatModel> Update(ChatModel chatModel, string tagNames);
      Task Delete(int id);
   }
}
