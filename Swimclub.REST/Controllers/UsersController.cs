using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Swimclub.REST.Controllers
{
	[Authorize]
	[Route("/[controller]")]
	[ApiController]
	public class UsersController : ControllerBase
	{
		[HttpGet("test")]
		public IActionResult Test()
		{
			return Ok();
		}
	}
}
