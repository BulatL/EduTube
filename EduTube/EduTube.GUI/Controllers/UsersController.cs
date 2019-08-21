using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using EduTube.BLL.Managers.Interfaces;
using EduTube.BLL.Models;
using EduTube.DAL.Entities;
using EduTube.GUI.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace EduTube.GUI.Controllers
{
   public class UsersController : Controller
   {
      private IApplicationUserManager _userManager;
      private SignInManager<ApplicationUser> _signInManager;
      private UserManager<ApplicationUser> _identityManager;

      public UsersController(IApplicationUserManager userManager, SignInManager<ApplicationUser> signInManager)
      {
         _userManager = userManager;
         _signInManager = signInManager;
      }

      public IActionResult Index()
      {
         return View();
      }

      [Route("Users/{channelName}")]
      public async Task<IActionResult> GetUserByChannelName(string channelName)
      {
         channelName = channelName.Replace("-", " ");

         ApplicationUserModel user = await _userManager.GetByChannelName(channelName);

         if (user == null)
            return StatusCode(404);

         //currentUser = await _identityManager.GetUserAsync(User);
         ApplicationUserModel currentUser = await _userManager.GetById(User.FindFirstValue(ClaimTypes.NameIdentifier), false);

         if (currentUser != null)
         {
            if (currentUser.Id == user.Id)
               ViewData["Subscribed"] = "SameUser";

            else
            {
               bool subscribedOn = false;
               if (user.Subscribers != null)
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
                  ViewData["Subscribed"] = "Unsubscribe";
               else
                  ViewData["Subscribed"] = "Subscribe";
            }
         }
         else
         {
            ViewData["Subscribed"] = "UserNotLogged";
         }

         return View("Profile", user);
      }

      [Route("Login")]
      public IActionResult Login()
      {
         return View(new LoginViewModel());
      }

      [Route("Login")]
      [HttpPost]
      public async Task<IActionResult> Login(LoginViewModel viewModel)
      {
         if (ModelState.IsValid)
         {
            var result = await _signInManager.PasswordSignInAsync(viewModel.Email, viewModel.Password, viewModel.RememberMe, lockoutOnFailure: true);
            if (result.Succeeded)
            {
               return RedirectToAction("Index", "Home");
            }
            /*if (result.RequiresTwoFactor)
            {
               return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
            }
            if (result.IsLockedOut)
            {
               _logger.LogWarning("User account locked out.");
               return RedirectToPage("./Lockout");
            }*/
            else
            {
               ModelState.AddModelError("email", "Invalid login attempt.");
               return View(viewModel);
            }
         }
         return View(viewModel);
      }

      [Route("Register")]
      public IActionResult Register()
      {
         return View(new RegisterViewModel());
      }

      [Route("Register")]
      [HttpPost]
      public async Task<IActionResult> Register(RegisterViewModel viewModel)
      {
         if (ModelState.IsValid)
         {
           //_identityManager.CreateAsync();
         }
         return View(new RegisterViewModel());
      }

      [Route("Logout")]
      public async Task<IActionResult> Logout()
      {
         await _signInManager.SignOutAsync();
         return RedirectToAction("Index", "Home");
      }
   }
}