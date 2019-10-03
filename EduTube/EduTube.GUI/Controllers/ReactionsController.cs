using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EduTube.BLL.Managers.Interfaces;
using EduTube.BLL.Models;
using EduTube.GUI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduTube.GUI.Controllers
{
   public class ReactionsController : Controller
   {
      private IReactionManager _reactionManager;
      private IApplicationUserManager _userManager;

      public ReactionsController(IReactionManager reactionManager, IApplicationUserManager userManager)
      {
         _reactionManager = reactionManager;
         _userManager = userManager;
      }

      [Authorize]
      [HttpPost]
      public async Task<IActionResult> Create(ReactionCreateViewModel reaction)
      {
         ApplicationUserModel currentUser = await _userManager.GetById(User.FindFirstValue(ClaimTypes.NameIdentifier), false);
         if (currentUser == null)
            return StatusCode(401);

         ReactionModel exist = new ReactionModel();
         ReactionModel model = ReactionCreateViewModel.CopyToModel(reaction);
         model.UserId = currentUser.Id;
         if (model.VideoId != null)
         {
            exist = await _reactionManager.GetByVideoAndUser(model.VideoId.Value, model.UserId);
         }
         else
         {
            exist = await _reactionManager.GetByCommentAndUser(model.CommentId.Value, model.UserId);
         }
         if (exist != null)
         {
            await _reactionManager.Remove(exist.Id);
            if (exist.EmojiId == model.EmojiId)
               return StatusCode(201);
         }
         await _reactionManager.Create(model);
         return StatusCode(201);
      }
   }
}