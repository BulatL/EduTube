using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using EduTube.BLL.Managers.Interfaces;
using EduTube.BLL.Models;
using System.Diagnostics;
using EduTube.GUI.ViewModels;
using EduTube.GUI.Services.Interface;
using System.Security.Claims;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;
using EduTube.DAL.Enums;
using System.Xml;
using EduTube.BLL.Enums;

namespace EduTube.GUI.Controllers
{
   public class VideosController : Controller
   {
      private readonly IVideoManager _videoManager;
      private readonly ITagRelationshipManager _tagRelationshipManager;
      private readonly ITagManager _tagManager;
      private readonly IApplicationUserManager _userManager;
      private readonly IUploadService _uploadService;
      private readonly IViewManager _viewManager;
      private readonly IReactionManager _reactionManager;
      private readonly IEmojiManager _emojiManager;
      private readonly IHostingEnvironment _hostingEnvironment;

      public VideosController(IVideoManager videoManager, ITagRelationshipManager tagRelationshipManager,
          ITagManager tagManager, IApplicationUserManager userManager,
          IUploadService uploadService, IViewManager viewManager, IReactionManager reactionManager,
          IEmojiManager emojiManager, IHostingEnvironment hostingEnvironment)
      {
         _videoManager = videoManager;
         _tagRelationshipManager = tagRelationshipManager;
         _tagManager = tagManager;
         _userManager = userManager;
         _uploadService = uploadService;
         _viewManager = viewManager;
         _reactionManager = reactionManager;
         _emojiManager = emojiManager;
         _hostingEnvironment = hostingEnvironment;
      }

      // GET: Videos
      public async Task<IActionResult> Index()
      {
         ApplicationUserModel user = await _userManager.GetById(User.FindFirstValue(ClaimTypes.NameIdentifier), false);

         return View(await _videoManager.GetTop5Videos(user?.Id));
      }

      [Route("Videos/RecommendedVideos/{ipAddress}")]
      public async Task<IActionResult> RecomendedVideos(string ipAddress)
      {
         List<VideoModel> firstRecommendedVideos = new List<VideoModel>();
         List<VideoModel> secondRecommendedVideos = new List<VideoModel>();
         TagModel firstTag = null;
         TagModel secondTag = null;

         Debug.WriteLine("usao u kontroler" + DateTime.Now);
         ApplicationUserModel user = await _userManager.GetById(User.FindFirstValue(ClaimTypes.NameIdentifier), false);
         Debug.WriteLine("nasao usera " + DateTime.Now);

         List<int> seenVideosId = await _videoManager.GetVideosIdByView(user?.Id, null);
         List<int?> tagsId = await _tagManager.Get2MostPopularTagsIdByVideoId(seenVideosId);
         if (tagsId != null && tagsId?.Count() > 0)
         {
            if (tagsId[0] != null)
            {
               firstTag = await _tagManager.GetById(int.Parse(tagsId[0].ToString()));
               firstRecommendedVideos = await _videoManager.Get6VideosByTag(user?.Id, tagsId[0]);
            }
            if (tagsId[1] != null)
            {
               secondTag = await _tagManager.GetById(int.Parse(tagsId[1].ToString()));
               secondRecommendedVideos = await _videoManager.Get6VideosByTag(user?.Id, tagsId[1]);
            }
         }

         Debug.WriteLine("nazad u kontroleru " + DateTime.Now);
         HomeRecommendedVideos viewModel = new HomeRecommendedVideos(firstRecommendedVideos,
             secondRecommendedVideos, firstTag.Name, secondTag.Name);
         return Json(viewModel);
      }

      [Route("Videos/{id}")]
      public async Task<IActionResult> SingleVideo(int id)
      {
         VideoModel video = await _videoManager.GetById(id, true);
         if (video != null)
         {
            ApplicationUserModel user = await _userManager.GetById(User.FindFirstValue(ClaimTypes.NameIdentifier), false);
            if (video.VideoVisibility == VideoVisibilityModel.Invitation)
            {
               if (user == null)
               {
                  ViewData["allowAccess"] = false;
               }
               else
               {
                  string userRole = await _userManager.GetCurrentUserRole(user.Id);
                  if (userRole.Equals("ADMIN") || user.Id.Equals(video.UserId))
                  {
                     ViewData["allowAccess"] = true;
                  }
                  else
                  {
                     ViewData["allowAccess"] = false;
                  }
               }
            }
            else if (video.VideoVisibility == VideoVisibilityModel.Private && user == null)
            {
               return LocalRedirect("/Login?redirectUrl=/Videos/" + id);
            }
            else
            {
               ViewData["allowAccess"] = true;
            }

            int? userReaction = await _emojiManager.GetEmojiId(id, user?.Id);
            List<ReactionModel> commentReactions = await _reactionManager.GetCommentsReactionsByUserAndVideo(video.Comments?.Select(x => x.Id).ToList(), user?.Id);

            return View(new VideoViewModel(video, userReaction, commentReactions));
         }
         return StatusCode(404);
      }

      [Route("Videos/Search/{search}")]
      public async Task<IActionResult> Search(string search)
      {
         ApplicationUserModel user = await _userManager.GetById(User.FindFirstValue(ClaimTypes.NameIdentifier), false);
         List<VideoModel> videos = await _videoManager.SearchVideos(user?.Id, search);
         return Json(videos);
      }

      // GET: Videos/Create
      [Authorize]
      public async Task<IActionResult> Create()
      {
         ViewData["HashtagsSelectList"] = new SelectList(await _tagManager.GetAll(), "Id", "Name");
         ViewBag.Hashtags = await _tagManager.GetAll();
         return View();
      }

      // POST: Videos/Create
      // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
      // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
      [Authorize]
      [HttpPost]
      //[ValidateAntiForgeryToken]
      [RequestFormLimits(ValueLengthLimit = int.MaxValue, MultipartBodyLengthLimit = int.MaxValue)]
      public async Task<IActionResult> Create(VideoCreateViewModel viewModel)
      {
         if (ModelState.IsValid)
         {
            VideoModel video = VideoCreateViewModel.ConvertToModel(viewModel);

            if (viewModel.Video != null)
            {
               video.FileName = await _uploadService.UploadVideo(viewModel.Video);
               Debug.WriteLine("Zavrsio sa snimanjem fajla" + DateTime.Now);
               video.Duration = _uploadService.VideoDuration(video.FileName);
            }
            else
               video.Duration = XmlConvert.ToTimeSpan(viewModel.VideoDuration);
            
            if(viewModel.Thumbnail != null)
               video.Thumbnail = _uploadService.UploadImage(viewModel.Thumbnail, "thumbnails");

            else
            {
               if (viewModel.Video != null)
                  video.Thumbnail = _uploadService.CreateThumbnail(video.FileName);
               else
                  video.Thumbnail = String.Format(@"https://img.youtube.com/vi/" + viewModel.YoutubeId + "/0.jpg");
            }
            if (video.VideoVisibility != VideoVisibilityModel.Invitation)
               video.InvitationCode = null;

            ApplicationUserModel currentUser = await _userManager.GetById(User.FindFirstValue(ClaimTypes.NameIdentifier), false);
            video.UserId = currentUser?.Id;
            video = await _videoManager.Create(video);
            return LocalRedirect("/Videos/" + video.Id);
         }
         return View(viewModel);
      }

      [Authorize]
      [HttpPost]
      public async Task<IActionResult> Edit(VideoViewModel viewModel, string InviteCode)
      {
         VideoModel video = VideoViewModel.CopyToModel(viewModel);
         video.InvitationCode = InviteCode;
         await _videoManager.Update(video);
         return LocalRedirect("/Videos/"+viewModel.Id);
      }
      
      public async Task<IActionResult> InvitationCode(int videoId,string invitationCode)
      {
         return Json(await _videoManager.CheckInvitationCode(videoId, invitationCode));
      }

      // GET: Videos/Delete/5
      [Authorize]
      [HttpDelete]
      [Route("Videos/Delete/{id}")]
      public async Task<IActionResult> Delete(int id)
      {
         int result = await _videoManager.Delete(id);
         if (result > 0)
            return StatusCode(200);
         else
            return StatusCode(404);
      }

   }
}
