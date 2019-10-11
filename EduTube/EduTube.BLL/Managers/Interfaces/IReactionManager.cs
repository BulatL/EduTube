using System.Collections.Generic;
using System.Threading.Tasks;
using EduTube.BLL.Models;

namespace EduTube.BLL.Managers.Interfaces
{
   public interface IReactionManager
   {
      Task<ReactionModel> GetByVideoAndUser(int videoId, string userId);
      Task<ReactionModel> GetByCommentAndUser(int commentId, string userId);
      Task<List<ReactionModel>> GetCommentsReactionsByUserAndVideo(List<int> commentsId, string userId);
      Task<ReactionModel> Create(ReactionModel reaction);
      Task Remove(int id);
   }
}
