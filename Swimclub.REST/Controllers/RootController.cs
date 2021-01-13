using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Swimclub.REST.Controllers
{
	[Route("/")]
	[ApiController]
	public class RootController : Controller
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
				message = "This is the root of the Swimclub REST API. Proceed to token exchange for authentication",
				token = new
				{
					href1 = Url.Link(nameof(TokenController.RootOfToken), null),
					//href = Url.Link(nameof(TokenController.TokenExchange), null)
				}
			};
			return Ok(response);
		}
	}
}
