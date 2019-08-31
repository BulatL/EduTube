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

namespace EduTube.GUI.Controllers
{
   public class ChatsController : Controller
   {
      private readonly ApplicationDbContext _context;
      private readonly IChatManager _chatManager;

      public ChatsController(ApplicationDbContext context, IChatManager chatManager)
      {
         _context = context;
         _chatManager = chatManager;
      }

      // GET: Chats
      public async Task<IActionResult> Index()
      {
         return View(await _chatManager.GetAll());
      }

      // GET: Chats/Details/5
      public async Task<IActionResult> Details(int? id)
      {
         if (id == null)
         {
            return NotFound();
         }

         var chat = await _context.Chats
             .FirstOrDefaultAsync(m => m.Id == id);
         if (chat == null)
         {
            return NotFound();
         }

         return View(chat);
      }

      // GET: Chats/Create
      public IActionResult Create()
      {
         return View();
      }

      // POST: Chats/Create
      // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
      // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Create([Bind("Id,Name,Deleted")] Chat chat)
      {
         if (ModelState.IsValid)
         {
            _context.Add(chat);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
         }
         return View(chat);
      }

      // GET: Chats/Edit/5
      public async Task<IActionResult> Edit(int? id)
      {
         if (id == null)
         {
            return NotFound();
         }

         var chat = await _chatManager.GetById(id.Value, true);
         if (chat == null)
         {
            return NotFound();
         }
         return View(chat);
      }

      // POST: Chats/Edit/5
      // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
      // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Edit(int id, ChatModel chat)
      {
         if (id != chat.Id)
         {
            return NotFound();
         }

         if (ModelState.IsValid)
         {
            ChatMessageModel m1 = new ChatMessageModel()
            {
               DateCreatedOn = DateTime.Now,
               Message = "nesto",
               UserId = "4bafedef-8780-491c-a6c7-c9d3d9c40fb8",
               Deleted = false
            };
            chat.Messages = new List<ChatMessageModel>();
            chat.Messages.Add(m1);
            await _chatManager.Update(chat);
            return RedirectToAction(nameof(Index));
         }
         return View(chat);
      }

      // GET: Chats/Delete/5
      public async Task<IActionResult> Delete(int? id)
      {
         if (id == null)
         {
            return NotFound();
         }

         var chat = await _context.Chats
             .FirstOrDefaultAsync(m => m.Id == id);
         if (chat == null)
         {
            return NotFound();
         }

         return View(chat);
      }

      // POST: Chats/Delete/5
      [HttpPost, ActionName("Delete")]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> DeleteConfirmed(int id)
      {
         var chat = await _context.Chats.FindAsync(id);
         _context.Chats.Remove(chat);
         await _context.SaveChangesAsync();
         return RedirectToAction(nameof(Index));
      }

      private bool ChatExists(int id)
      {
         return _context.Chats.Any(e => e.Id == id);
      }
   }
}
