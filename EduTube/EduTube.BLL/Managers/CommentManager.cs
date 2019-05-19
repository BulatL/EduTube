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
        private ApplicationDbContext _context;

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
                .Include(x => x.Reactions).Include(x => x.User).Include(x => x.Video)
                .FirstOrDefaultAsync(x => x.Id == id && !x.Deleted))
                : CommentMapper.EntityToModel(await _context.Comments
                .FirstOrDefaultAsync(x => x.Id == id && !x.Deleted));
        }

        public async Task<CommentModel> Create(CommentModel comment)
        {
            Comment entity = CommentMapper.ModelToEntity(comment);
            _context.Comments.Add(entity);
            await _context.SaveChangesAsync();
            return CommentMapper.EntityToModel(entity);
        }

        public async Task<CommentModel> Update(CommentModel comment)
        {
            _context.Update(CommentMapper.ModelToEntity(comment));
            await _context.SaveChangesAsync();
            return comment;
        }

        public async Task Delete(int id)
        {
            Comment entity = await _context.Comments.FirstOrDefaultAsync(x => x.Id == id);
            /*entity.Deleted = true;
            _context.Update(entity);*/
            _context.Comments.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
