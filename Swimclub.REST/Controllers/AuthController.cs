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
		/// <returns>A <see cref="AuthResponse"/></returns>
		/// 
		[HttpPost("login", Name = nameof(Login))]
		[ProducesResponseType(StatusCodes.Status200OK, Type=typeof(AuthResponse)), ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(AuthResponse))]
		public async Task<ActionResult<AuthResponse>> Login([FromBody] Models.Login loginInfo)
		{
			Services.UserService.UserServiceResponse feedback = await user_service.LoginUserAsync(loginInfo);
			AuthResponse temp = new AuthResponse() { Success = feedback.success };
			if (feedback.success)
			{
				temp.Token = feedback.token;
				temp.Expiry = feedback.ExpireDate;
				return Ok(temp);
			}
			else
			{
				temp.Error = new Models.ApiError() { Message = "Incorrect login credentials", Detail = "Failed authentication", Success = false };
				return Unauthorized(temp);
			}
		}



		public class AuthResponse
		{
			public bool Success { get; set; }
			public string Token { get; set; }
			public DateTime? Expiry { get; set; }
			public Models.ApiError Error { get; set; }
		}
	}
}
