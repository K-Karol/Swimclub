using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Swimclub.Data;
namespace Swimclub.REST.Controllers
{
	[Route("/[controller]")]
	[ApiController]
	public class RoomsController : Controller
	{

		private readonly HotelApiDbContext context;
		public RoomsController(HotelApiDbContext _context)
		{
			context = _context;
		}

		[HttpGet(Name = nameof(GetRooms))]
		public IActionResult GetRooms()
		{
			throw new NotImplementedException();
		}


		// GET /rooms/{roomID}
		[HttpGet("{roomID}", Name = nameof(GetRoomByID))]
		public async Task<ActionResult<Models.Room>> GetRoomByID(Guid roomID)
		{
			var entity = await context.Rooms.SingleOrDefaultAsync(x => x.ID == roomID);
			if (entity == null)
				return NotFound();
			var resource = new Models.Room
			{
				ID = entity.ID,
				Name = entity.Name,
				Rate = entity.Rate / 100.0m
			};
			return resource;
		}
	}
}
