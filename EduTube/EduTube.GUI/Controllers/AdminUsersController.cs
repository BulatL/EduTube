using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduTube.BLL.Managers.Interfaces;
using EduTube.BLL.Models;
using EduTube.DAL.Entities;
using EduTube.GUI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduTube.GUI.Controllers
{
   [Authorize(Roles = "Admin")]
   public class AdminUsersController : Controller
   {
      private readonly IApplicationUserManager _userManager;

      public AdminUsersController(IApplicationUserManager userManager)
      {
         _userManager = userManager;
      }

      public async Task<IActionResult> Index()
      {
         List<ApplicationUser> users = await _userManager.GetAll(10, 0);
         List<AdminUserViewModel> viewModels = new List<AdminUserViewModel>();
         foreach (ApplicationUser user in users)
         {
            string role = await _userManager.GetRole(user);
            AdminUserViewModel viewModel = new AdminUserViewModel(user.Id, user.ChannelName, user.Blocked, role, user.ProfileImage);
            viewModels.Add(viewModel);
         }
         return View(viewModels);
      }

      public async Task<IActionResult> BlockUnblock(bool block, string id)
      {
         await _userManager.BlockUnblock(id, block);
         return StatusCode(200);
      }

      public async Task<IActionResult> PromoteDemote(bool promote,string id)
      {
         await _userManager.PromoteDemote(id, promote);
         return StatusCode(200);
      }
   }
}