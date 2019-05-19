using System.Collections.Generic;
using System.Threading.Tasks;
using EduTube.BLL.Models;

namespace EduTube.BLL.Managers.Interfaces
{
    interface IEmoticonManager
    {
        Task<List<EmoticonModel>> GetAll();
        Task<EmoticonModel> GetById(int id);
        Task<EmoticonModel> Create(EmoticonModel emoticon);
        Task<EmoticonModel> Update(EmoticonModel emoticon);
        Task Delete(int id);
    }
}
