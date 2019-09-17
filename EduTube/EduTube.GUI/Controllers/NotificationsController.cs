using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EduTube.BLL.Managers.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduTube.GUI.Controllers
{
   [Authorize]
   public class NotificationsController : Controller
   {
      private readonly INotificationManager _notificationManager;

      public NotificationsController(INotificationManager notificationManager)
      {
         _notificationManager = notificationManager;
      }

      public async Task<IActionResult> Index()
      {
         return View(await _notificationManager.GetLast5ByUser(User.FindFirstValue(ClaimTypes.NameIdentifier)));
      }

      public async Task<IActionResult> LoadMore(int skip)
      {
         return Json(await _notificationManager.Get5ByUser(User.FindFirstValue(ClaimTypes.NameIdentifier), skip));
      }

      public async Task<IActionResult> GetLast5()
      {
         return Json(await _notificationManager.GetLast5ByUser(User.FindFirstValue(ClaimTypes.NameIdentifier)));
      }

      public async Task<IActionResult> GetNewNotifications(int lastId)
      {
         return Json(await _notificationManager.GetNewNotifications(User.FindFirstValue(ClaimTypes.NameIdentifier), lastId));
      }
   }
}