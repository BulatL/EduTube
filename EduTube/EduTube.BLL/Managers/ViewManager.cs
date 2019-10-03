using EduTube.BLL.Managers.Interfaces;
using EduTube.BLL.Models;
using EduTube.DAL.Data;
using EduTube.BLL.Mappers;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using EduTube.DAL.Entities;
using System.Linq;

namespace EduTube.BLL.Managers
{
   public class ViewManager : IViewManager
   {
      private ApplicationDbContext _context;

      public ViewManager(ApplicationDbContext context)
      {
         _context = context;
      }

      public async Task<int> CountViewsByVideo(int videoId)
      {
         return await _context.Views.CountAsync(x => x.VideoId == videoId);
      }

      public async Task<bool> ViewExist(int videoId, string userId, string ipAddress)
      {
         return await _context.Views.AnyAsync(x => x.VideoId == videoId && (x.UserId.Equals(userId) || x.IpAddress.Equals(ipAddress)));
      }

      public async Task<ViewModel> Create(int videoId, string userId, string ipAddress)
      {
         View entity = new View();
         entity.VideoId = videoId;
         if (userId == null)
            entity.IpAddress = ipAddress;
         else
            entity.UserId = userId;
         _context.Views.Add(entity);
         await _context.SaveChangesAsync();
         return ViewMapper.EntityToModel(entity);
      }
   }
}
