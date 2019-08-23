using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduTube.BLL.Managers.Interfaces;
using EduTube.BLL.Models;
using EduTube.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EduTube.GUI.Controllers
{
   [Authorize]
   public class SubscribeController : Controller
   {
      private ISubscriptionManager _subscriptionManager;
      private UserManager<ApplicationUser> _userManager;

      public SubscribeController(ISubscriptionManager subscriptionManager, UserManager<ApplicationUser> userManager)
      {
         _subscriptionManager = subscriptionManager;
         _userManager = userManager;
      }

      [Route("Subscribe/{subscribeOn}/{method}")]
      public async Task<IActionResult> Subscribe(string subscribeOn, string method)
      {
         ApplicationUser currentUser = new ApplicationUser();
         currentUser = await _userManager.GetUserAsync(User);


         if (method.Equals("Subscribe"))
         {
            SubscriptionModel subscription = new SubscriptionModel()
            {
               Deleted = false,
               SubscriberId = currentUser.Id,
               SubscribedOnId = subscribeOn
            };
            subscription = await _subscriptionManager.Create(subscription);
            if (subscription.Id != 0)
               return Json((currentUser.ChannelName, currentUser.ProfileImage));

            return Json("Error");
         }
         else if (method.Equals("Unsubscribe"))
         {
            await _subscriptionManager.Remove(currentUser.Id, subscribeOn);

            return Json(currentUser.ChannelName);
         }
         return Json("Error");
      }
   }
}