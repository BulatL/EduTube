using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace EduTube.GUI.Controllers
{
	public class ErrorController : Controller
	{
		[Route("Error/404")]
		public IActionResult Error404()
		{
			return View();
		}
		[Route("Error/400")]
		public IActionResult Error400()
		{
			return View();
		}
		[Route("Error/401")]
		public IActionResult Error401()
		{
			return View();
		}
		[Route("Error/403")]
		public IActionResult Error403()
		{
			return View();
		}
	}
}