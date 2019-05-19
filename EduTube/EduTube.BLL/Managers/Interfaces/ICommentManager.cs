using System.Collections.Generic;
using System.Threading.Tasks;
using EduTube.BLL.Models;

namespace EduTube.BLL.Managers.Interfaces
{
    public interface ICommentManager
    {
        Task<List<CommentModel>> GetAll();
        Task<CommentModel> GetById(int id, bool includeAll);
        Task<CommentModel> Create(CommentModel comment);
        Task<CommentModel> Update(CommentModel comment);
        Task Delete(int id);
    }
}
