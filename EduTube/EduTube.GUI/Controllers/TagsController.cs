using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduTube.BLL.Managers.Interfaces;
using EduTube.BLL.Models;
using Microsoft.AspNetCore.Mvc;

namespace EduTube.GUI.Controllers
{
   public class TagsController : Controller
   {

      private readonly ITagManager _tagManager;
      public TagsController(ITagManager tagManager)
      {
         _tagManager = tagManager;
      }

      public async Task<IActionResult> GetAll()
      {
         List<TagModel> tags = await _tagManager.GetAll();
         return Json(tags.Select(x => x.Name));
      }

   }
}