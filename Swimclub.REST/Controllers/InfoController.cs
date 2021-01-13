using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Swimclub.REST.Controllers
{
	[Route("/[controller]")]
	[ApiController]
	public class InfoController : Controller
	{
		private readonly Models.HotelInfo hotelInfo;
		public InfoController(IOptions<Models.HotelInfo> _hotelWrapper)
		{
			hotelInfo = _hotelWrapper.Value;
		}

		[HttpGet(Name = nameof(GetInfo))]
		public ActionResult<Models.HotelInfo> GetInfo()
		{
			return hotelInfo;
		}
	}
}
