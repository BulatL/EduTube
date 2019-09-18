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
      private readonly ISubscriptionManager _subscriptionManager;

      public VideosController(IVideoManager videoManager, ITagRelationshipManager tagRelationshipManager,
          ITagManager tagManager, IApplicationUserManager userManager,
          IUploadService uploadService, IViewManager viewManager, IReactionManager reactionManager,
          IEmojiManager emojiManager, IHostingEnvironment hostingEnvironment, ISubscriptionManager subscriptionManager)
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
         _subscriptionManager = subscriptionManager;
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

         List<int> seenVideosId = await _videoManager.GetVideosIdByView(user?.Id, ipAddress);
         List<int?> tagsId = await _tagManager.Get2MostPopularTagsIdByVideoId(seenVideosId);
         if (tagsId != null && tagsId?.Count() > 0)
         {
            if (tagsId.ElementAtOrDefault(0) != null)
            {
               firstTag = await _tagManager.GetById(int.Parse(tagsId[0].ToString()));
               firstRecommendedVideos = await _videoManager.Get6VideosByTag(user?.Id, tagsId.ElementAtOrDefault(0));
            }
            if (tagsId.ElementAtOrDefault(1) != null)
            {
               secondTag = await _tagManager.GetById(int.Parse(tagsId[1].ToString()));
               secondRecommendedVideos = await _videoManager.Get6VideosByTag(user?.Id, tagsId.ElementAtOrDefault(1));
            }
         }

         Debug.WriteLine("nazad u kontroleru " + DateTime.Now);
         HomeRecommendedVideos viewModel = new HomeRecommendedVideos(firstRecommendedVideos,
             secondRecommendedVideos, firstTag.Name, secondTag.Name);
         return Json(viewModel);
      }

      public IActionResult InvitationCode(string videoId)
      {
         ViewBag.VideoId = videoId;
         return View();
      }

      public async Task<IActionResult> CheckInvitationCode(int videoId, string invitationCode)
      {
         return Json(await _videoManager.CheckInvitationCode(videoId, invitationCode));
      }

      [Route("Videos/{id}")]
      public async Task<IActionResult> SingleVideo(int id)
      {
         VideoModel video = await _videoManager.GetById(id, true);
         if (video != null)
         {
            ApplicationUserModel user = await _userManager.GetById(User.FindFirstValue(ClaimTypes.NameIdentifier), false);
				bool allowAccess = true;
				string subscribed = "";
            #region Video visibility
            if (video.VideoVisibility == VideoVisibilityModel.Invitation)
            {
               if (user == null)
               {
                  ViewBag.allowAccess = false;
                  //return RedirectToAction("InvitationCode", new { videoId = id });
               }
               else
               {
                  string userRole = await _userManager.GetCurrentUserRole(user.Id);
                  if (userRole.Equals("ADMIN") || user.Id.Equals(video.UserId))
                  {
							allowAccess = true;
                  }
                  else
                  {
							allowAccess = false;
                  }

               }
            }
            else if (video.VideoVisibility == VideoVisibilityModel.Private && user == null)
            {
               return LocalRedirect("/Login?redirectUrl=/Videos/" + id);
            }
            else
            {
					allowAccess = true;
            }
#endregion
            #region current user subscribed
            if (user == null)
            {
					subscribed = "UserNotLogged";
            }
            else
            {
               if (video.UserId.Equals(user.Id))
               {
						subscribed = "SameUser";
               }
               else
               {
                  bool isSubscribed = await _subscriptionManager.IsUserSubscribed(video.UserId, user.Id);
                  if (isSubscribed)
                  {
							subscribed = "Unsubscribe";
                  }
                  else
                  {
							subscribed = "Subscribe";
                  }
               }
            }
            #endregion

            int? userReaction = await _emojiManager.GetEmojiId(id, user?.Id);
            List<ReactionModel> commentReactions = await _reactionManager.GetCommentsReactionsByUserAndVideo(video.Comments?.Select(x => x.Id).ToList(), user?.Id);

            return View(new VideoViewModel(video, userReaction, commentReactions, allowAccess, subscribed));
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
      public IActionResult Create()
      {
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
               video.YoutubeUrl = null;
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
            video.UserChannelName = currentUser.ChannelName;
            video.User = currentUser;
            List<TagModel> tags = await _tagManager.GetByNames(viewModel.Tags);
            List<TagRelationshipModel> trs = new List<TagRelationshipModel>();
            foreach (var item in tags)
            {
               TagRelationshipModel tr = new TagRelationshipModel() { Id = 0, TagId = item.Id, Tag = item, Video = video };
               trs.Add(tr);
            }
            video.TagRelationships = trs;
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
