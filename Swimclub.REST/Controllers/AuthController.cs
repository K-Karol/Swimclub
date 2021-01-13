using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Swimclub.REST.Controllers
{
	[Route("/[controller]")]
	[ApiController]
	public class AuthController : Controller
	{
		private readonly Services.IUserService user_service;
		public AuthController(Services.IUserService _user_service) 
		{
			user_service = _user_service;
		}
		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] Models.Login loginInfo)
		{
			Models.ApiError feedback = await user_service.LoginUserAsync(loginInfo);
			if (feedback.Success)
			{
				return Ok(feedback);
			}
			else
			{
				return StatusCode(401,feedback);
			}
		}
	}
}
