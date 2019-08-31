using System.Collections.Generic;
using System.Threading.Tasks;
using EduTube.BLL.Models;

namespace EduTube.BLL.Managers.Interfaces
{
   public interface IReactionManager
   {
      Task<List<ReactionModel>> GetAll();
      Task<ReactionModel> GetById(int id);
      Task<List<ReactionModel>> GetByVideo(int videoId);
      Task<ReactionModel> GetByVideoAndUser(int videoId, string userId);
      Task<ReactionModel> GetByCommentAndUser(int commentId, string userId);
      Task<List<ReactionModel>> GetCommentsReactionsByUserAndVideo(List<int> commentsId, string userId);
      Task<ReactionModel> Create(ReactionModel reaction);
      Task<ReactionModel> Update(ReactionModel reaction);
      Task Delete(int id);
      Task Remove(int id);
      Task DeleteActivateByUser(string id, bool option);
   }
}
