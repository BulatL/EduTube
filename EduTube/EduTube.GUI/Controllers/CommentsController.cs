using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EduTube.DAL.Data;
using EduTube.DAL.Entities;
using EduTube.BLL.Managers.Interfaces;
using EduTube.GUI.ViewModels;
using EduTube.BLL.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace EduTube.GUI.Controllers
{
   public class CommentsController : Controller
   {
      private readonly ICommentManager _commentManager;
      private readonly IApplicationUserManager _userManager;

      public CommentsController(ICommentManager commentManager, IApplicationUserManager userManager)
      {
         _commentManager = commentManager;
         _userManager = userManager;
      }

      // POST: Comments/Create
      // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
      // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
      [Authorize]
      [HttpPost]
      public async Task<IActionResult> Create(CreateCommentViewModel viewModel)
      {
         if (ModelState.IsValid)
         {
            ApplicationUserModel user = await _userManager.GetById(User.FindFirstValue(ClaimTypes.NameIdentifier), false);
            CommentModel comment = new CommentModel()
            {
               Content = viewModel.CommentContent,
               DateCreatedOn = DateTime.Now,
               Deleted = false,
               VideoId = viewModel.VideoId,
               UserId = user.Id
            };
            comment = await _commentManager.Create(comment);
            return Json(new { commentId = comment.Id, ownerProfileImage = user.ProfileImage, ownerChannelName = user.ChannelName,
               dateCreatedOn = comment.DateCreatedOn, commentContent = comment.Content});
         }
         return StatusCode(400);
      }

      [Authorize]
      [HttpPut]
      [Route("Comments/Edit/{id}")]
      public async Task<IActionResult> Edit(int id, CreateCommentViewModel viewModel)
      {
         string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
         CommentModel comment = await _commentManager.GetById(id, false);

         if (!userId.Equals(comment.UserId))
            return StatusCode(400);

         await _commentManager.Update(id, viewModel.CommentContent);

         return StatusCode(200);
      }

      [Authorize]
      [HttpDelete]
      [Route("Comments/Delete/{id}")]
      public async Task<IActionResult> Delete(int id)
      {
         int result = await _commentManager.Delete(id);
         if (result > 0 )
            return StatusCode(200);
         else
            return StatusCode(404);
      }

   }
}
