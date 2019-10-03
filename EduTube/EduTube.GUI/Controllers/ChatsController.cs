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
using EduTube.BLL.Mappers;
using EduTube.BLL.Managers.Interfaces;
using EduTube.BLL.Models;
using EduTube.GUI.ViewModels;

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
      public IActionResult Create()
      {
         return View();
      }

      // POST: Chats/Create
      // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
      // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
      [Authorize(Roles = "Admin")]
      [AutoValidateAntiforgeryToken]
      [HttpPost]
      public async Task<IActionResult> Create(ChatViewModel viewModel)
      {
         if (ModelState.IsValid)
         {
            ChatModel chat = new ChatModel()
            {
               Name = viewModel.Name,
               Deleted = false,
               TagRelationships = new List<TagRelationshipModel>()
            };
            List<TagModel> tags = await _tagManager.GetByNames(viewModel.Tags);
            foreach (var tag in tags)
            {
               TagRelationshipModel relationshipModel = new TagRelationshipModel(){ Id = 0, Tag = tag, TagId = tag.Id, Chat = chat};
               chat.TagRelationships.Add(relationshipModel);
            }
            await _chatManager.Create(chat);
            return RedirectToAction("Index");
         }
         return StatusCode(401);
      }
      
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

            List<TagModel> tags = await _tagManager.GetByNames(viewModel.Tags);
            List<TagRelationshipModel> oldRelationships = await _tagRelationshipManager.GetByChat(chat.Id);
            foreach (var tag in tags)
            {
               TagRelationshipModel oldRelationship = oldRelationships.FirstOrDefault(x => x.TagId == tag.Id);
               if (oldRelationship == null)
               {
                  TagRelationshipModel relationshipModel = new TagRelationshipModel() { Id = 0, Tag = tag, TagId = tag.Id, Chat = chat };
                  chat.TagRelationships.Add(relationshipModel);
               }
               else
               {
                  oldRelationships.Remove(oldRelationship);
                  chat.TagRelationships.Add(oldRelationship);
               }
            }
            foreach (var item in oldRelationships)
            {
               await _tagRelationshipManager.Remove(item.Id);
            }
            await _chatManager.Update(chat);
            return RedirectToAction("Index");
         }
         return View(viewModel);
      }

      // GET: Chats/Delete/5
      [Authorize(Roles = "Admin")]
      [HttpDelete]
      public async Task<IActionResult> Delete(int? id)
      {
         await _chatManager.Delete(id.Value);
         return Ok();
      }
   }
}
