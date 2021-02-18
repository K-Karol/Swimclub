using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Swimclub.REST.Controllers
{
	/// <summary>
	/// This controller is responsible for all interactions with system's users
	/// </summary>
	[Authorize]
	[Route("/[controller]")]
	[ApiController]
	public class UsersController : ControllerBase
	{
		private readonly Services.IUserService user_service;
		private readonly IAuthorizationService authService;
		public UsersController(Services.IUserService _user_service, IAuthorizationService _authService)
		{
			user_service = _user_service;
			authService = _authService;
		}
		/// <summary>
		/// Root of this controller
		/// </summary>
		/// <returns>Controller's endpoints</returns>
		[HttpGet(Name=nameof(UserRoot))]
		[ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status401Unauthorized)]
		public IActionResult UserRoot()
		{
			return Ok(new
			{
				message = "This endpoint is responsible for all interactions with system's users",
				userinfo = new
				{
					href = Url.Link(nameof(UserInfo), null)
				},
				

			}); ;
			}

		/// <summary>
		/// This will respond with the information about the current user.
		/// </summary>
		/// <returns><see cref="Models.UserInfoResponse"/></returns>
		[HttpGet("info", Name = nameof(UserInfo))]
		[ProducesResponseType(StatusCodes.Status400BadRequest, Type=typeof(Models.UserInfoResponse)), ProducesResponseType(StatusCodes.Status401Unauthorized), ProducesResponseType(StatusCodes.Status200OK, Type=typeof(Models.UserInfoResponse))]
		public async Task<ActionResult<Models.UserInfoResponse>> UserInfo() {
			var user = await user_service.GetUserAsync(User);
			if (user == null)
			{
				return BadRequest(new Models.UserInfoResponse() { error = new Models.ApiError() { Success = false, Message = "Invalid Grant", Detail = "This user does not exist" } });
			}
			return Ok(new Models.UserInfoResponse() { forename = user.Forename, surname = user.Surname, username = user.Username });
		}



		[HttpPost("register", Name = nameof(RegisterUser))]
		public async Task<ActionResult<Models.CreateUserResponse>> RegisterUser([FromBody] Models.Register _register)
		{
			Models.CreateUserResponse response;
			if (User.Identity.IsAuthenticated)
			{
				var policyCheck = await authService.AuthorizeAsync(User, "AdminPolicy");
				if (!policyCheck.Succeeded)
				{
					return Unauthorized(response = new Models.CreateUserResponse() { Success = false, Error = new Models.ApiError() { Success = false, Message = "This user is not authorised.", Detail = "Requirements: Admin" } });

				}
			}

			Models.UserServiceRegistrationResponse resp = await user_service.RegisterUserAsync(_register);
			if (!resp.Success)
			{
				return BadRequest(new Models.CreateUserResponse() { Success = false, Error = new Models.ApiError() { Success = false, Message = "There was an error creating the user", Detail = JsonSerializer.Serialize(resp.PasswordValidErrors) } });
			}

			return Ok(new Models.CreateUserResponse() { Success = true, Error = new Models.ApiError() { Success = true } });

		}
	}

	
}
