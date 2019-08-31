using EduTube.BLL.Managers.Interfaces;
using EduTube.BLL.Models;
using EduTube.DAL.Data;
using EduTube.BLL.Mappers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduTube.DAL.Entities;

namespace EduTube.BLL.Managers
{
   public class CommentManager : ICommentManager
   {
      private readonly ApplicationDbContext _context;

      public CommentManager(ApplicationDbContext context)
      {
         _context = context;
      }

      public async Task<List<CommentModel>> GetAll()
      {
         return CommentMapper.EntitiesToModels(await _context.Comments.Where(x => !x.Deleted).ToListAsync());
      }

      public async Task<CommentModel> GetById(int id, bool includeAll)
      {
         return includeAll ? CommentMapper.EntityToModel(await _context.Comments
             .Include(x => x.Reactions)
             .Include(x => x.User)
             .FirstOrDefaultAsync(x => x.Id == id && !x.Deleted))
             : CommentMapper.EntityToModel(await _context.Comments
             .FirstOrDefaultAsync(x => x.Id == id && !x.Deleted));
      }
      public async Task<List<CommentModel>> GetByVideo(int videoId)
      {
         return CommentMapper.EntitiesToModels(await _context.Comments
             .Include(x => x.Reactions)
             .Include(x => x.User)
             .Where(x => x.VideoId == videoId && !x.Deleted).ToListAsync());
      }

      public async Task<CommentModel> Create(CommentModel comment)
      {
         Comment entity = CommentMapper.ModelToEntity(comment);
         _context.Comments.Add(entity);
         await _context.SaveChangesAsync();
         return CommentMapper.EntityToModel(entity);
      }

      public async Task<CommentModel> Update(int id, string content)
      {
         Comment entity = await _context.Comments.FirstOrDefaultAsync(x => x.Id == id);
         entity.Content = content;
         _context.Update(entity);
         await _context.SaveChangesAsync();
         return CommentMapper.EntityToModel(entity);
      }

      public async Task<int> Delete(int id)
      {
         Comment entity = await _context.Comments.FirstOrDefaultAsync(x => x.Id == id);
         entity.Deleted = true;
         _context.Update(entity);
         return await _context.SaveChangesAsync();
      }
      public async Task Remove(int id)
      {
         Comment entity = await _context.Comments.FirstOrDefaultAsync(x => x.Id == id);
         _context.Comments.Remove(entity);
         await _context.SaveChangesAsync();
      }
   }
}
