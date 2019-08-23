using EduTube.BLL.Managers.Interfaces;
using EduTube.BLL.Models;
using EduTube.DAL.Data;
using EduTube.BLL.Mappers;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using EduTube.DAL.Entities;

namespace EduTube.BLL.Managers
{
   public class ViewManager : IViewManager
   {
      private ApplicationDbContext _context;

      public ViewManager(ApplicationDbContext context)
      {
         _context = context;
      }

      public async Task<List<ViewModel>> GetAll()
      {
         return ViewMapper.EntitiesToModels(await _context.Views.ToListAsync());
      }

      public async Task<ViewModel> GetById(int id)
      {
         return ViewMapper.EntityToModel(await _context.Views
             .FirstOrDefaultAsync(x => x.Id == id));
      }

      public async Task<ViewModel> Create(ViewModel view)
      {
         View entity = ViewMapper.ModelToEntity(view);
         _context.Views.Add(entity);
         await _context.SaveChangesAsync();
         return ViewMapper.EntityToModel(entity);
      }

      public async Task<ViewModel> Update(ViewModel view)
      {
         _context.Update(ViewMapper.ModelToEntity(view));
         await _context.SaveChangesAsync();
         return view;
      }

      public async Task Delete(int id)
      {
         View entity = await _context.Views.FirstOrDefaultAsync(x => x.Id == id);
         _context.Views.Remove(entity);
         await _context.SaveChangesAsync();
      }
   }
}
