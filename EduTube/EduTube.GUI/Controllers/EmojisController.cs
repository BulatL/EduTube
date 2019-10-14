using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EduTube.DAL.Data;
using EduTube.DAL.Entities;
using EduTube.BLL.Managers.Interfaces;
using EduTube.BLL.Models;
using Microsoft.AspNetCore.Authorization;
using EduTube.GUI.ViewModels;
using EduTube.GUI.Services.Interface;

namespace EduTube.GUI.Controllers
{
	[Authorize(Roles = "Admin")]
	public class EmojisController : Controller
	{
		private readonly IEmojiManager _emojiManager;
		private readonly IUploadService _uploadService;

		public EmojisController(IEmojiManager emojiManager, IUploadService uploadService)
		{
			_emojiManager = emojiManager;
			_uploadService = uploadService;
		}

		// GET: Emojis
		public async Task<IActionResult> Index()
		{
			return View(await _emojiManager.GetAll());
		}

		[Route("/Emoji/{id}")]
		public async Task<IActionResult> Emoji(int id)
		{
			EmojiModel emoji = await _emojiManager.GetById(id);
			if (emoji == null)
			{
				return NotFound();
			}

			return View(emoji);
		}

		// GET: Emojis/Create
		public IActionResult Create()
		{
			return View(new EmojiViewModel());
		}

		// POST: Emojis/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(EmojiViewModel viewModel)
		{
			if (ModelState.IsValid)
			{
				EmojiModel emoji = EmojiViewModel.CopyToModel(viewModel);

				if(viewModel.Image != null)
					emoji.ImagePath = _uploadService.UploadImage(viewModel.Image, "images");
				
				await _emojiManager.Create(emoji);
				return RedirectToAction(nameof(Index));
			}
			return View(viewModel);
		}

		// GET: Emojis/Edit/5
		public async Task<IActionResult> Edit(int id)
		{

			EmojiModel emoji = await _emojiManager.GetById(id);
			if (emoji == null)
			{
				return NotFound();
			}
			EmojiViewModel viewModel = new EmojiViewModel(emoji);
			return View(viewModel);
		}

		// POST: Emojis/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, EmojiViewModel viewModel)
		{
			if (id != viewModel.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				EmojiModel emoji = EmojiViewModel.CopyToModel(viewModel);

				if (viewModel.Image != null)
					emoji.ImagePath = _uploadService.UploadImage(viewModel.Image, "images");

				await _emojiManager.Update(emoji);
				return RedirectToAction(nameof(Index));
			}
			return View(viewModel);
		}

		[Route("Emojis/GetDeleteDialog/{id}")]
		public async Task<IActionResult> GetDeleteDialog(int id)
		{
			bool exist = await _emojiManager.Exist(id);
			if (!exist)
				return StatusCode(404);

			ViewData["type"] = "emoji";
			ViewData["id"] = id;
			return PartialView("DeleteModalDialog");
		}

		[HttpDelete]
		public async Task<IActionResult> Delete(int id)
		{
			await _emojiManager.Remove(id);
			return Ok();
		}
	}
}
