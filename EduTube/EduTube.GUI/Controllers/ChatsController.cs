using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using EduTube.BLL.Managers.Interfaces;
using EduTube.BLL.Models;
using EduTube.GUI.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace EduTube.GUI.Controllers
{
   public class ChatsController : Controller
   {
      private readonly IChatManager _chatManager;
      private readonly IChatMessageManager _chatMessageManager;
      private readonly ITagManager _tagManager;
      private readonly ITagRelationshipManager _tagRelationshipManager;

      public ChatsController(IChatManager chatManager, IChatMessageManager chatMessageManager,
         ITagManager tagManager, ITagRelationshipManager tagRelationshipManager)
      {
         _chatManager = chatManager;
         _chatMessageManager = chatMessageManager;
         _tagManager = tagManager;
         _tagRelationshipManager = tagRelationshipManager;

      }

      // GET: Chats
      [Authorize]
      public async Task<IActionResult> Index()
      {
         return View(await _chatManager.GetAll());
      }

      [Route("Chats/GetMessages/{id}")]
      public async Task<IActionResult> GetMessages(int id)
      {
         return Json(await _chatMessageManager.GetByChat(id));
      }

		// GET: Chats/Create
		[Authorize(Roles = "Admin")]
		public IActionResult Create()
      {
         return View();
      }

      [Authorize(Roles = "Admin")]
      [AutoValidateAntiforgeryToken]
      [HttpPost]
      public async Task<IActionResult> Create(ChatViewModel viewModel)
      {
         if (ModelState.IsValid)
         {
            ChatModel chat = ChatViewModel.CopyToModel(viewModel);
            await _chatManager.Create(chat, viewModel.Tags);
            return RedirectToAction("Index");
         }
			return LocalRedirect("/Error/401");
		}

		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Edit(int id)
      {
         ChatModel chat = await _chatManager.GetById(id);
         if (chat == null)
            return NotFound();

         return View(new ChatViewModel(chat));
      }

      [Authorize(Roles = "Admin")]
      [AutoValidateAntiforgeryToken]
      [HttpPost]
      public async Task<IActionResult> Edit(ChatViewModel viewModel)
      {
         if (ModelState.IsValid)
         {
            ChatModel chat = ChatViewModel.CopyToModel(viewModel);
         
            await _chatManager.Update(chat, viewModel.Tags);
            return RedirectToAction("Index");
         }
         return View(viewModel);
      }

      [Authorize(Roles = "Admin")]
      [HttpDelete]
      public async Task<IActionResult> Delete(int? id)
      {
         await _chatManager.Delete(id.Value);
         return Ok();
      }
   }
}
