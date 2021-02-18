using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Swimclub.REST.Controllers
{
	/// <summary>
	/// The <see cref="AuthController"/> is responsible for authorization 
	/// </summary>
	[Route("/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly Services.IUserService user_service;
		public AuthController(Services.IUserService _user_service) 
		{
			user_service = _user_service;
		}
		/// <summary>
		/// This will accept a <see cref="Models.Login"/> with a username and password, and will proceed to authenticate the user, responding with either a 200 and a token, or a 401 and an error message.
		/// </summary>
		/// <param name="loginInfo"><see cref="Models.Login"/> which contains the username and password</param>
		/// <returns>A <see cref="Models.AuthResponse"/></returns>
		/// 
		[HttpPost("login", Name = nameof(Login))]
		[ProducesResponseType(StatusCodes.Status200OK, Type=typeof(Models.AuthResponse)), ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(Models.AuthResponse))]
		public async Task<ActionResult<Models.AuthResponse>> Login([FromBody] Models.Login loginInfo)
		{
			Models.AuthResponse feedback = await user_service.LoginUserAsync(loginInfo);
			return feedback;
		}



		
	}
}
