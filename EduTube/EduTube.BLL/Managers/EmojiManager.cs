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
   public class EmojiManager : IEmojiManager
   {
      private ApplicationDbContext _context;

      public EmojiManager(ApplicationDbContext context)
      {
         _context = context;
      }

      public async Task<int?> GetEmojiId(int videoId, string userId)
      {
         return await _context.Reactions.Where(x => x.VideoId == videoId && x.UserId.Equals(userId) && !x.Deleted).Select(x => x.EmojiId).FirstOrDefaultAsync();
      }
   }
}
