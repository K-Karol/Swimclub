using AspNet.Security.OpenIdConnect.Primitives;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Swimclub.REST.Controllers
{
	[Route("/[controller]")]
	[ApiController]
	public class TokenController : Controller
	{
		[HttpGet(Name = nameof(RootOfToken))]
		public IActionResult RootOfToken()
		{
			return StatusCode(403);
		}

		//public IActionResult TokenExchange(OpenIdConnectRequest)
		//{
		//	return Ok();
		//}
	}
}
