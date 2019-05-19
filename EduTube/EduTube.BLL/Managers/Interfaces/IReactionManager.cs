using System.Collections.Generic;
using System.Threading.Tasks;
using EduTube.BLL.Models;

namespace EduTube.BLL.Managers.Interfaces
{
    public interface IReactionManager
    {
        Task<List<ReactionModel>> GetAll();
        Task<ReactionModel> GetById(int id);
        Task<ReactionModel> Create(ReactionModel reaction);
        Task<ReactionModel> Update(ReactionModel reaction);
        Task Delete(int id);
    }
}
