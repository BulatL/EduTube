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
using Microsoft.AspNetCore.Identity;
using EduTube.BLL.Managers.Interfaces;
using EduTube.BLL.Models;
using System.Diagnostics;
using EduTube.GUI.ViewModels;

namespace EduTube.GUI.Controllers
{
    public class VideosController : Controller
    {
        private IVideoManager _videoManager;
        private IHashtagRelationshipManager _hashtagRelationshipManager;
        private IHashtagManager _hashtagManager;
        private UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public VideosController(IVideoManager videoManager, IHashtagRelationshipManager hashtagRelationshipManager,
            IHashtagManager hashtagManager, UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _videoManager = videoManager;
            _hashtagRelationshipManager = hashtagRelationshipManager;
            _hashtagManager = hashtagManager;
            _userManager = userManager;
            _context = context;
        }

        // GET: Videos
        public async Task<IActionResult> Index()
        {
            ApplicationUser user = await _userManager.GetUserAsync(User);
            List<VideoModel> videos = new List<VideoModel>();
            if(user != null)
            {
                return View(await _videoManager.GetTop5Videos(user.Id));
            }
            else
            {
                return View(await _videoManager.GetTop5Videos(null));
            }
        }
        [Route("Videos/RecommendedVideos/{ipAddress}")]
        public async Task<IActionResult> RecomendedVideos(string ipAddress)
        {

            List<VideoModel> firstRecommendedVideos = new List<VideoModel>();
            List<VideoModel> secondRecommendedVideos = new List<VideoModel>();
            List<int> videosId = new List<int>();
            List<int?> hashtagsId = new List<int?>();
            HashtagModel firstHashtag = null;
            HashtagModel secondHashtag = null;

            Debug.WriteLine("usao u kontroler" + DateTime.Now);
            ApplicationUser user = new ApplicationUser();
            user = await _userManager.GetUserAsync(User);
            Debug.WriteLine("nasao usera " + DateTime.Now);


            if (user != null)
            {
                videosId = await _videoManager.GetVideosIdByView(user.Id, null);
                hashtagsId = await _hashtagManager.Get2MostPopularHashtagsIdByVideoId(videosId);
                if(hashtagsId[0] != null)
                {
                    firstHashtag = await _hashtagManager.GetById(int.Parse(hashtagsId[0].ToString()));
                    firstRecommendedVideos = await _videoManager.Get6VideosByHashtag(user.Id, hashtagsId[0]);
                }
                if (hashtagsId[1] != null)
                {
                    secondHashtag = await _hashtagManager.GetById(int.Parse(hashtagsId[1].ToString()));
                    secondRecommendedVideos = await _videoManager.Get6VideosByHashtag(user.Id, hashtagsId[1]);
                }
            }

            else
            {
                videosId = await _videoManager.GetVideosIdByView(null, ipAddress);
                hashtagsId = await _hashtagManager.Get2MostPopularHashtagsIdByVideoId(videosId);
                if (hashtagsId[0] != null)
                {
                    firstHashtag = await _hashtagManager.GetById(int.Parse(hashtagsId[0].ToString()));
                    firstRecommendedVideos = await _videoManager.Get6VideosByHashtag(null, hashtagsId[0]);
                }
                if (hashtagsId[1] != null)
                {
                    secondHashtag = await _hashtagManager.GetById(int.Parse(hashtagsId[1].ToString()));
                    secondRecommendedVideos = await _videoManager.Get6VideosByHashtag(null, hashtagsId[1]);
                }
            }

            Debug.WriteLine("nazad u kontroleru " + DateTime.Now);
            HomeRecommendedVideos viewModel = new HomeRecommendedVideos(firstRecommendedVideos,
                secondRecommendedVideos, firstHashtag.Name, secondHashtag.Name);
            return Json(viewModel);
        }

        [Route("Videos/Search/{search}")]
        public async Task<IActionResult> Search(string search)
        {
            ApplicationUser user = await _userManager.GetUserAsync(User);
            List<VideoModel> videos = await _videoManager.SearchVideos(user?.Id, search);
            return Json(videos);
        }

        // GET: Videos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var video = await _context.Videos
                .Include(v => v.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (video == null)
            {
                return NotFound();
            }

            return View(video);
        }

        // GET: Videos/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Videos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,DateCreatedOn,YoutubeUrl,FilePath,AllowComments,Blocked,Deleted,IvniteCode,UserId,VideoVisibility")] Video video)
        {
            if (ModelState.IsValid)
            {
                _context.Add(video);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", video.UserId);
            return View(video);
        }

        // GET: Videos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var video = await _context.Videos.FindAsync(id);
            if (video == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", video.UserId);
            return View(video);
        }

        // POST: Videos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,DateCreatedOn,YoutubeUrl,FilePath,AllowComments,Blocked,Deleted,IvniteCode,UserId,VideoVisibility")] Video video)
        {
            if (id != video.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(video);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VideoExists(video.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", video.UserId);
            return View(video);
        }

        // GET: Videos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var video = await _context.Videos
                .Include(v => v.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (video == null)
            {
                return NotFound();
            }

            return View(video);
        }

        // POST: Videos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var video = await _context.Videos.FindAsync(id);
            _context.Videos.Remove(video);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VideoExists(int id)
        {
            return _context.Videos.Any(e => e.Id == id);
        }
    }
}
