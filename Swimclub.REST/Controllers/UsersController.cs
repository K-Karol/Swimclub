using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
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

		public UsersController(Services.IUserService _user_service)
		{
			user_service = _user_service;
		}


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
		/// <returns><see cref="UserInfoResponse"/></returns>
		[HttpGet("info", Name = nameof(UserInfo))]
		[ProducesResponseType(StatusCodes.Status400BadRequest, Type=typeof(UserInfoResponse)), ProducesResponseType(StatusCodes.Status401Unauthorized), ProducesResponseType(StatusCodes.Status200OK, Type=typeof(UserInfoResponse))]
		public async Task<ActionResult<UserInfoResponse>> UserInfo() {
			var user = await user_service.GetUserAsync(User);
			if (user == null)
			{
				return BadRequest(new UserInfoResponse() { error = new Models.ApiError() { Success = false, Message = "Invalid Grant", Detail = "This user does not exist" } });
			}
			return Ok(new UserInfoResponse() { forename = user.Forename, surname = user.Surname, username = user.Username });
		}
	}

	public class UserInfoResponse
	{
		public string username { get; set; }
		public string forename { get; set; }
		public string surname { get; set; }
		public Models.ApiError error { get; set; }
	}
}
