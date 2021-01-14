using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Swimclub.REST.Controllers
{
	/// <summary>
	/// Starting point of the API. 
	/// </summary>
	[Route("/")]
	[ApiController]
	public class RootController : ControllerBase
	{
		[HttpGet(Name = nameof(GetRoot))]
		public IActionResult GetRoot()
		{
			//var response = new
			//{
			//	href = Url.Link(nameof(GetRoot), null),
			//	rooms = new
			//	{
			//		href = Url.Link(nameof(RoomsController.GetRooms), null)
			//	},
			//	info = new
			//	{
			//		href = Url.Link(nameof(InfoController.GetInfo), null)
			//	}
			//};
			//return Ok(response);

			var response = new
			{
				message = "This is the root of the Swimclub REST API. Proceed to token endpoint for authorization",
				authorization = new
				{
					href = Url.Link(nameof(AuthController.Login), null),
				}
			};
			return Ok(response);
		}
	}
}
