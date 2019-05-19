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
    public class ReactionManager : IReactionManager
    {
        private ApplicationDbContext _context;

        public ReactionManager(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ReactionModel>> GetAll()
        {
            return ReactionMapper.EntitiesToModels(await _context.Reactions.Where(x => !x.Deleted).ToListAsync());
        }

        public async Task<ReactionModel> GetById(int id)
        {
            return ReactionMapper.EntityToModel(await _context.Reactions
                .FirstOrDefaultAsync(x => x.Id == id && !x.Deleted));
        }

        public async Task<ReactionModel> Create(ReactionModel reaction)
        {
            Reaction entity = ReactionMapper.ModelToEntity(reaction);
            _context.Reactions.Add(entity);
            await _context.SaveChangesAsync();
            return ReactionMapper.EntityToModel(entity);
        }

        public async Task<ReactionModel> Update(ReactionModel reaction)
        {
            _context.Update(ReactionMapper.ModelToEntity(reaction));
            await _context.SaveChangesAsync();
            return reaction;
        }

        public async Task Delete(int id)
        {
            Reaction entity = await _context.Reactions.FirstOrDefaultAsync(x => x.Id == id);
            /*entity.Deleted = true;
            _context.Update(entity);*/
            _context.Reactions.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
