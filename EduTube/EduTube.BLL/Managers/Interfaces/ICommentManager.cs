using System.Collections.Generic;
using System.Threading.Tasks;
using EduTube.BLL.Models;

namespace EduTube.BLL.Managers.Interfaces
{
   public interface ICommentManager
   {
      Task<List<CommentModel>> GetAll();
      Task<List<CommentModel>> GetByVideo(int videoId, int lastCommentId);
      Task<CommentModel> GetById(int id, bool includeAll);
      Task<CommentModel> Create(CommentModel comment);
      Task<CommentModel> Update(int id, string content);
      Task<int> Delete(int id);
      Task Remove(int id);
   }
}
