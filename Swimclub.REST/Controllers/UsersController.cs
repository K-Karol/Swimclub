using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
		private readonly Data.UserDbContext userDbContext;
		private readonly IAuthorizationService authService;
		public UsersController(Services.IUserService _user_service, IAuthorizationService _authService, Data.UserDbContext _userDbContext)
		{
			user_service = _user_service;
			authService = _authService;
			userDbContext = _userDbContext;
		}
		/// <summary>
		/// Root of this controller
		/// </summary>
		/// <returns>Controller's endpoints</returns>
		[HttpGet(Name = nameof(UserRoot))]
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
		[Authorize]
		[ProducesResponseType(StatusCodes.Status400BadRequest, Type=typeof(Models.UserInfoResponse)), ProducesResponseType(StatusCodes.Status401Unauthorized), ProducesResponseType(StatusCodes.Status200OK, Type=typeof(Models.UserInfoResponse))]
		public async Task<ActionResult<Models.UserInfoResponse>> UserInfo() {
			var user = await user_service.GetUserAsync(User);
			if (user == null)
			{
				return BadRequest(new Models.UserInfoResponse() { Success = false, Error = new Models.ApiError() { Message = "Invalid user", Detail = "Cannot retrieve user", Code= 0 } });
			}
			return Ok(new Models.UserInfoResponse() { Success = true, User = new Models.standard.Item<Models.User>() { itemValue = user } });
		}



		[HttpPost("register", Name = nameof(RegisterUser))]
		public async Task<ActionResult<Models.RegistrationResponse>> RegisterUser([FromBody] Models.Register _register)
		{
			Models.RegistrationResponse response;
			if (User.Identity.IsAuthenticated)
			{
				var policyCheck = await authService.AuthorizeAsync(User, "AdminPolicy");
				if (!policyCheck.Succeeded)
				{
					return Unauthorized(response = new Models.RegistrationResponse() { Success = false, Error = Models.ApiError.UnAuthResponse() });

				}
			}

			Models.RegistrationResponse resp = await user_service.RegisterUserAsync(_register);
			if (!resp.Success)
			{
				return BadRequest(resp);
			}

			return Ok(resp);

		}
		[HttpGet("all",Name = nameof(GetAllUsers))]
		public async Task<ActionResult<Models.AllUsersResponse>> GetAllUsers()
		{
			Models.AllUsersResponse response;
			if (User.Identity.IsAuthenticated)
			{
				var policyCheck = await authService.AuthorizeAsync(User, "AdminPolicy");
				if (!policyCheck.Succeeded)
				{
					return Unauthorized(response = new Models.AllUsersResponse() { Success = false, Error = Models.ApiError.UnAuthResponse()});
				}
			}

			Entities.User[] entities = await userDbContext.Users.ToArrayAsync();
			Models.User[] users = entities.Select(ent => Entities.User.getUser(ent)).ToArray();
			Models.standard.Collection<Models.User> ret = new Models.standard.Collection<Models.User>() { values = users, length = users.Length };
			response = new Models.AllUsersResponse() { Success = true, Users = ret };
			return Ok(response);
		}
	}

	
}
