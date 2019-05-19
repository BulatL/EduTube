using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EduTube.GUI.Models;
using EduTube.BLL.Managers.Interfaces;
using Microsoft.AspNetCore.Identity;
using EduTube.DAL.Entities;
using EduTube.BLL.Models;
using Microsoft.AspNetCore.Authorization;

namespace EduTube.GUI.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private IVideoManager _videoManager;
        private UserManager<ApplicationUser> _userManager;

        public HomeController(IVideoManager videoManager, UserManager<ApplicationUser> userManager)
        {
            _videoManager = videoManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            ApplicationUser user = await _userManager.GetUserAsync(User);
            if(user == null)
            {
                List<VideoModel> videos = await _videoManager.GetAll();
                return Json(videos);
            }
            else
            {
                List<VideoModel> videos = await _videoManager.GetAll();
                return Json(videos);
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
