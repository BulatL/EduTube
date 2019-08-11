using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using EduTube.BLL.Managers.Interfaces;
using EduTube.BLL.Models;
using EduTube.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace EduTube.GUI.Controllers
{
    public class UsersController : Controller
    {
        private IApplicationUserManager _userManager2;
        private UserManager<ApplicationUser> _userManager;

        public UsersController(IApplicationUserManager userManager2, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _userManager2 = userManager2;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("Users/{channelName}")]
        public async Task<IActionResult> GetUserByChannelName(string channelName)
        {
            channelName = channelName.Replace("-", " ");

            ApplicationUserModel user = await _userManager2.GetByChannelName(channelName);

            if (user == null)
                return StatusCode(404);

            ApplicationUser currentUser = new ApplicationUser();
            currentUser = await _userManager.GetUserAsync(User);

            if (currentUser != null)
            {
                if(currentUser.Id == user.Id)
                    ViewData["Subscribed"] = "SameUser";

                else
                {
                    bool subscribedOn = false;
                    if(user.Subscribers != null)
                    {
                        foreach (SubscriptionModel subscriber in user.Subscribers)
                        {
                            if (subscriber.SubscriberId == currentUser.Id)
                            {
                                subscribedOn = true;
                                break;
                            }
                        }
                    }
                    if (subscribedOn)
                        ViewData["Subscribed"] = "Subscribed";
                    else
                        ViewData["Subscribed"] = "NotSubscribed";
                }
            }
            else
            {
                ViewData["Subscribed"] = "UserNotLogged";  
            }

            return View("Profile", user);
        }
    }
}