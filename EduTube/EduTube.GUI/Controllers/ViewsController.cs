using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EduTube.BLL.Managers.Interfaces;
using EduTube.BLL.Models;
using Microsoft.AspNetCore.Mvc;

namespace EduTube.GUI.Controllers
{
   public class ViewsController : Controller
   {
      private IViewManager _viewManager;
      private IApplicationUserManager _userManager;

      public ViewsController(IViewManager viewManager, IApplicationUserManager userManager)
      {
         _viewManager = viewManager;
         _userManager = userManager;
      }

      [Route("Views")]
      public async Task<IActionResult> GetByVideo(int videoId, string ipAddress)
      {
         ApplicationUserModel currentUser = await _userManager.GetById(User.FindFirstValue(ClaimTypes.NameIdentifier), false);
         bool exist = await _viewManager.ViewExist(videoId, currentUser?.Id, ipAddress);
         if (!exist)
            await _viewManager.Create(videoId, currentUser?.Id, ipAddress);
         
         int videoCount = await _viewManager.CountViewsByVideo(videoId);
         return Json(videoCount);
      }
   }
}