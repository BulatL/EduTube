﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using EduTube.BLL.Managers.Interfaces;
using EduTube.BLL.Models;
using EduTube.DAL.Entities;
using EduTube.GUI.Services.Interface;
using EduTube.GUI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace EduTube.GUI.Controllers
{
   public class UsersController : Controller
   {
      private IApplicationUserManager _userManager;
      private IUploadService _uploadSerice;

      public UsersController(IApplicationUserManager userManager, IUploadService uploadSerice)
      {
         _userManager = userManager;
         _uploadSerice = uploadSerice;
      }

      public IActionResult Index()
      {
         return View();
      }

      [Route("Users/{channelName}")]
      public async Task<IActionResult> UserProfile(string channelName)
      {
         channelName = channelName.Replace("-", " ");

         ApplicationUserModel currentUser = await _userManager.GetById(User.FindFirstValue(ClaimTypes.NameIdentifier), false);
         string userRole = await _userManager.GetCurrentUserRole(currentUser?.Id);

         ApplicationUserModel user = await _userManager.GetByChannelName(channelName, currentUser?.Id, userRole);

         if (user == null)
            return StatusCode(404);

         if (user.Videos != null)
            user.Videos = user.Videos.OrderBy(x => x.Id).ToList();

         //currentUser = await _identityManager.GetUserAsync(User);

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
      public IActionResult Login(string redirectUrl)
      {
         return View(new LoginViewModel() { RedirectUrl = redirectUrl});
      }

      [Route("Login")]
      [HttpPost]
      public async Task<IActionResult> Login(LoginViewModel viewModel)
      {
         if (ModelState.IsValid)
         {
            var result = await _userManager.Login(viewModel.Email, viewModel.Password, viewModel.RememberMe);
            if (result)
            {
               if (viewModel.RedirectUrl == "" || viewModel.RedirectUrl == null)
                  return RedirectToAction("Index", "Home");
               else
                  return LocalRedirect(viewModel.RedirectUrl);
            }
            /*if (result.Succeeded)
            {
               //var x = await _userManager.GetByEmailAndPassword(viewModel.Email, viewModel.Password);
               /*ApplicationUserModel user = await _userManager.GetByEmail(viewModel.Email);
               ViewBag.profileImage = user.ProfileImage;*/
            /*if (viewModel.ReturnUrl == "" || viewModel.ReturnUrl == null)
               return RedirectToAction("Index", "Home");
            else
               return LocalRedirect(viewModel.ReturnUrl);
         }*/
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

      [Authorize]
      [Route("Users/Edit/{id}")]
      public async Task<IActionResult> Edit(string id)
      {
         ApplicationUserModel currentUser = await _userManager.GetById(User.FindFirstValue(ClaimTypes.NameIdentifier), false);
         if (id.Equals(currentUser?.Id))
         {
            EditUserViewModel viewModel = new EditUserViewModel(currentUser);
            return View(viewModel);
         }
         return StatusCode(401);
      }

      [Authorize]
      [Route("Users/Edit/{id}")]
      [HttpPost]
      public async Task<IActionResult> Edit(string id, EditUserViewModel viewModel)
      {
         if (ModelState.IsValid)
         {
            ApplicationUserModel user = EditUserViewModel.CopyToModel(viewModel);
            if(viewModel.ProfileImage != null)
               user.ProfileImage = _uploadSerice.UploadImage(viewModel.ProfileImage, "profileImages");

            user = await _userManager.Update(user);
            return LocalRedirect("/Users/" + user.ChannelName);
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
            ApplicationUserModel user = new ApplicationUserModel()
            {
               Blocked = false,
               Deleted = false,
               Lastname = viewModel.Lastname,
               Firstname = viewModel.Firstname,
               ChannelDescription = viewModel.ChannelDescription,
               ChannelName = viewModel.ChannelName,
               DateOfBirth = viewModel.DateOfBirth,
               Email = viewModel.Email,
               UserName = viewModel.Email
            };

            if (viewModel.ProfileImage != null)
               user.ProfileImage = _uploadSerice.UploadImage(viewModel.ProfileImage, "profileImages");

            if (user.ProfileImage == null || user.ProfileImage == "")
               user.ProfileImage = "default-avatar.png";

            var result = await _userManager.Register(user, viewModel.Password);
            foreach (var error in result)
            {
               if(error.Code.Equals("PasswordTooShort"))
                  ModelState.AddModelError("Password", error.Description);

               if (error.Code.Equals("ChannelnameInPassword"))
                  ModelState.AddModelError("Password", error.Description);

               if (error.Code.Equals("UppercaseCharacterInPassword"))
                  ModelState.AddModelError("Password", error.Description);

               if (error.Code.Equals("NonUppercaseCharacterInPassword"))
                  ModelState.AddModelError("Password", error.Description);

               if (error.Code.Equals("UppercaseCharacterInPassword"))
                  ModelState.AddModelError("Password", error.Description);

               if (error.Code.Equals("DuplicateUserName"))
                  ModelState.AddModelError("Email", error.Description);

               return View(viewModel);

            }
            return RedirectToAction("Login");
         }
         return View(viewModel);
      }

      public async Task<IActionResult> ChannelNameEmailExist(string channelName, string email, string userId)
      {
         bool ex = await _userManager.ChannelNameExist(channelName, userId);
         bool ce = await _userManager.EmailExist(email, userId);
         return Json(new { channelNameExist = ex, emailExist = ce });
      }

      [Route("Logout")]
      public async Task<IActionResult> Logout()
      {
         await _userManager.Logout();
         return RedirectToAction("Index", "Home");
      }

      [Authorize]
      [HttpDelete]
      [Route("Users/Delete/{id}")]
      public async Task<IActionResult> Delete(string id)
      {
         IdentityResult result = await _userManager.Delete(id);
         if (result.Succeeded)
         {
            await _userManager.Logout();
            return StatusCode(200);
         }
         else
            return StatusCode(404);
      }
   }
}