using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using EduTube.BLL.Enums;
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
      private IVideoManager _videoManager;
      private ITagRelationshipManager _tagRelationshipManager;

      public UsersController(IApplicationUserManager userManager, IUploadService uploadSerice,
         IVideoManager videoManager, ITagRelationshipManager tagRelationshipManager)
      {
         _userManager = userManager;
         _uploadSerice = uploadSerice;
         _videoManager = videoManager;
         _tagRelationshipManager = tagRelationshipManager;
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

         UserProfileViewModel viewModel = new UserProfileViewModel(user);
         viewModel.Role = await _userManager.GetRole(user.Id);
         return View("Profile", viewModel);
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
            LoginResult result = await _userManager.Login(viewModel.Email, viewModel.Password, viewModel.RememberMe);
            if (result == LoginResult.Success)
            {
               if (viewModel.RedirectUrl == "" || viewModel.RedirectUrl == null)
                  return RedirectToAction("Index", "Home");
               else
                  return LocalRedirect(viewModel.RedirectUrl);
            }
            else if(result == LoginResult.UserBlocked)
            {
               ModelState.AddModelError("email", "You'r Channel is bloccked");
               return View(viewModel);
            }
            else if(result == LoginResult.WrongCredentials || result == LoginResult.UserNotFound)
            {
               ModelState.AddModelError("email", "Invalid login attempt.");
               return View(viewModel);
            }
            else if(result == LoginResult.ClaimsFailed)
            {
               ModelState.AddModelError("email", "There was problem with you'r login, please try again later.");
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
			return LocalRedirect("/Error/401");
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
            await new ElasticsearchController(_videoManager, _userManager, _tagRelationshipManager).UpdateUser(user);
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

            else
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
            await new ElasticsearchController(_videoManager, _userManager, _tagRelationshipManager).IndexUser(user);
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

      [Route("Users/GetDeleteDialog/{id}")]
      public async Task<IActionResult> GetDeleteDialog(string id)
      {
         bool exist = await _userManager.Exist(id);
         if (!exist)
            return StatusCode(404);

         ViewData["type"] = "user";
         ViewData["id"] = id;
         return PartialView("DeleteModalDialog");
      }

      [Authorize]
      [HttpDelete]
      [Route("Users/Delete/{id}")]
      public async Task<IActionResult> Delete(string id)
      {
         IdentityResult result = await _userManager.Delete(id);
         if (result.Succeeded)
         {
            if(id.Equals(User.FindFirstValue(ClaimTypes.NameIdentifier)))
               await _userManager.Logout();
            new ElasticsearchController(_videoManager, _userManager, _tagRelationshipManager).DeleteDocument(id, "users", "applicationusermodel");
            return StatusCode(200);
         }
         else
            return StatusCode(404);
      }

      [Authorize]
      [Route("Users/ChangePassword/{id}")]
      public async Task<IActionResult> ChangePassword(string id)
      {
         string currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
         string currentUserRole = await _userManager.GetRole(currentUserId);

         if (id.Equals(currentUserId) || (!id.Equals(currentUserId) && currentUserRole.Equals("Admin")))
         {
            ChangePasswordViewModel viewModel = new ChangePasswordViewModel(id);
            return View(viewModel);
         }

         else
			{
				return LocalRedirect("/Error/401");
			}
      }
      [Authorize]
      [HttpPost]
      [Route("Users/ChangePassword/{id}")]
      public async Task<IActionResult> ChangePassword(ChangePasswordViewModel viewModel)
      {
         if (ModelState.IsValid)
         {
            string currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string currentUserRole = await _userManager.GetRole(currentUserId);

            if (viewModel.UserId.Equals(currentUserId) || (!viewModel.UserId.Equals(currentUserId) && currentUserRole.Equals("Admin")))
            {
               IdentityResult changePassword = await _userManager.ChangePassword(viewModel.UserId, viewModel.OldPassword, viewModel.NewPassword);
               if (!changePassword.Succeeded)
               {
                  ModelState.AddModelError("OldPassword", "Password doesn't match old password");
                  return View(viewModel);
               }
               return LocalRedirect("/Users/" + User.Claims.FirstOrDefault(x => x.Type.Equals("channelName")).Value);
            }
            else
				{
					return LocalRedirect("/Error/401");
				}
         }
         else
         {
            return View(viewModel);
         }
      }
   }
}