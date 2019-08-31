﻿using EduTube.BLL.Managers.Interfaces;
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
      private readonly ApplicationDbContext _context;

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

      public async Task<List<ReactionModel>> GetByVideo(int videoId)
      {
         return ReactionMapper.EntitiesToModels(await _context.Reactions.Where(x => x.VideoId == videoId && !x.Deleted).ToListAsync());
      }

      public async Task<ReactionModel> GetByVideoAndUser(int videoId, string userId)
      {
         return ReactionMapper.EntityToModel(await _context.Reactions.FirstOrDefaultAsync(x => x.VideoId == videoId && 
         x.UserId.Equals(userId) && !x.Deleted));
      }
      public async Task<ReactionModel> GetByCommentAndUser(int commentId, string userId)
      {
         return ReactionMapper.EntityToModel(await _context.Reactions.FirstOrDefaultAsync(x => x.CommentId == commentId &&
         x.UserId.Equals(userId) && !x.Deleted));
      }

      public async Task<List<ReactionModel>> GetCommentsReactionsByUserAndVideo(List<int> commentsId, string userId)
      {
         return ReactionMapper.EntitiesToModels(await _context.Reactions.Where(x => commentsId.Contains(x.CommentId.Value) &&
         x.UserId.Equals(userId) && !x.Deleted).ToListAsync());
      }

      public async Task<ReactionModel> Create(ReactionModel reaction)
      {
         Reaction entity = ReactionMapper.ModelToEntity(reaction);
         await _context.Reactions.AddAsync(entity);
         try
         {
            await _context.SaveChangesAsync();
         }
         catch(Exception e)
         {
            Console.WriteLine(e);
         }
         return ReactionMapper.EntityToModel(entity);
      }

      public async Task<ReactionModel> Update(ReactionModel reaction)
      {
         _context.Update(ReactionMapper.ModelToEntity(reaction));
         await _context.SaveChangesAsync();
         return reaction;
      }

      public async Task Remove(int id)
      {
         Reaction entity = await _context.Reactions.FirstOrDefaultAsync(x => x.Id == id);
         /*entity.Deleted = true;
         _context.Update(entity);*/
         _context.Reactions.Remove(entity);
         await _context.SaveChangesAsync();
      }
      public async Task Delete(int id)
      {
         Reaction entity = await _context.Reactions.FirstOrDefaultAsync(x => x.Id == id);
         entity.Deleted = true;
         _context.Update(entity);
         await _context.SaveChangesAsync();
      }

      public async Task DeleteActivateByUser(string id, bool option)
      {
         List<Reaction> reactions = await _context.Reactions.Where(x => x.UserId.Equals(id) && x.Deleted == !option).ToListAsync();
         reactions.Select(x => x.Deleted = option);
         _context.UpdateRange(reactions);
         await _context.SaveChangesAsync();
      }
   }
}
