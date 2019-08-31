using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EduTube.DAL.Data;
using EduTube.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using EduTube.BLL.Managers.Interfaces;
using EduTube.BLL.Models;
using System.Diagnostics;
using EduTube.GUI.ViewModels;
using EduTube.GUI.Services.Interface;
using System.Security.Claims;

namespace EduTube.GUI.Controllers
{
   public class VideosController : Controller
   {
      private readonly IVideoManager _videoManager;
      private readonly IHashtagRelationshipManager _hashtagRelationshipManager;
      private readonly IHashtagManager _hashtagManager;
      private readonly IApplicationUserManager _userManager;
      private readonly ApplicationDbContext _context;
      private readonly IUploadService _uploadService;
      private readonly IViewManager _viewManager;
      private readonly IReactionManager _reactionManager;
      private readonly IEmoticonManager _emoticonManager;
      private readonly ICommentManager _commentManager;

      public VideosController(IVideoManager videoManager, IHashtagRelationshipManager hashtagRelationshipManager,
          IHashtagManager hashtagManager, IApplicationUserManager userManager, ApplicationDbContext context,
          IUploadService uploadService, IViewManager viewManager, IReactionManager reactionManager,
          IEmoticonManager emoticonManager, ICommentManager commentManager)
      {
         _videoManager = videoManager;
         _hashtagRelationshipManager = hashtagRelationshipManager;
         _hashtagManager = hashtagManager;
         _userManager = userManager;
         _context = context;
         _uploadService = uploadService;
         _viewManager = viewManager;
         _reactionManager = reactionManager;
         _emoticonManager = emoticonManager;
         _commentManager = commentManager;
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
         HashtagModel firstHashtag = null;
         HashtagModel secondHashtag = null;

         Debug.WriteLine("usao u kontroler" + DateTime.Now);
         ApplicationUserModel user = await _userManager.GetById(User.FindFirstValue(ClaimTypes.NameIdentifier), false);
         Debug.WriteLine("nasao usera " + DateTime.Now);

         List<int> seenVideosId = await _videoManager.GetVideosIdByView(user?.Id, null);
         List<int?> hashtagsId = await _hashtagManager.Get2MostPopularHashtagsIdByVideoId(seenVideosId);
         if (hashtagsId != null)
         {
            if (hashtagsId[0] != null)
            {
               firstHashtag = await _hashtagManager.GetById(int.Parse(hashtagsId[0].ToString()));
               firstRecommendedVideos = await _videoManager.Get6VideosByHashtag(user?.Id, hashtagsId[0]);
            }
            if (hashtagsId[1] != null)
            {
               secondHashtag = await _hashtagManager.GetById(int.Parse(hashtagsId[1].ToString()));
               secondRecommendedVideos = await _videoManager.Get6VideosByHashtag(user?.Id, hashtagsId[1]);
            }
         }

         Debug.WriteLine("nazad u kontroleru " + DateTime.Now);
         HomeRecommendedVideos viewModel = new HomeRecommendedVideos(firstRecommendedVideos,
             secondRecommendedVideos, firstHashtag.Name, secondHashtag.Name);
         return Json(viewModel);
      }
      [Route("Videos/{id}")]
      public async Task<IActionResult> SingleVideo(int id)
      {
         VideoModel video = await _videoManager.GetById(id, true);
         ApplicationUserModel user = await _userManager.GetById(User.FindFirstValue(ClaimTypes.NameIdentifier), false);
         int? userReaction = await _emoticonManager.GetEmoticonId(id, user?.Id);
         List<ReactionModel> commentReactions = await _reactionManager.GetCommentsReactionsByUserAndVideo(video.Comments?.Select(x => x.Id).ToList(), user?.Id);
         
         return View(new VideoViewModel(video, userReaction, commentReactions));
      }


      [Route("Videos/Search/{search}")]
      public async Task<IActionResult> Search(string search)
      {
         ApplicationUserModel user = await _userManager.GetById(User.FindFirstValue(ClaimTypes.NameIdentifier), false);
         List<VideoModel> videos = await _videoManager.SearchVideos(user?.Id, search);
         return Json(videos);
      }

      // GET: Videos/Create
      public IActionResult Create()
      {
         return View();
      }

      // POST: Videos/Create
      // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
      // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
      [Authorize]
      [HttpPost]
      [ValidateAntiForgeryToken]
      [RequestFormLimits(ValueLengthLimit = int.MaxValue, MultipartBodyLengthLimit = int.MaxValue)]
      public async Task<IActionResult> Create(VideoCreateViewModel viewModel)
      {
         if (ModelState.IsValid)
         {
            VideoModel video = VideoCreateViewModel.ConvertToModel(viewModel);

            if (viewModel.Video != null)
            {
               video.FilePath = await _uploadService.UploadVideo(viewModel.Video);
               Debug.WriteLine("Zavrsio sa snimanjem fajla" + DateTime.Now);
               video.Duration = _uploadService.VideoDuration(video.FilePath);
            }
            ApplicationUserModel currentUser = await _userManager.GetById(User.FindFirstValue(ClaimTypes.NameIdentifier), false);
            video.UserId = currentUser.Id;
            await _videoManager.Create(video);
            return RedirectToAction(nameof(Index));
         }
         return View(viewModel);
      }
      [Authorize]
      [HttpPost]
      public async Task<IActionResult> Edit(VideoViewModel viewModel, string InviteCode)
      {
         VideoModel video = VideoViewModel.CopyToModel(viewModel);
         video.IvniteCode = InviteCode;
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
