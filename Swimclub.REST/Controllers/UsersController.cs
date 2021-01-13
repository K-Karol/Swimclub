using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Swimclub.REST.Controllers
{
	[Route("/[controller]")]
	[ApiController]
	public class UsersController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
