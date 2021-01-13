using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Swimclub.REST.Demo
{
	public static class SeedData
	{

		public static async Task InitialiseAsync(IServiceProvider services)
		{
			//await AddTestDataHotel(services.GetRequiredService<HotelApiDbContext>());
			if (LaunchFlags.Instance.NewHost)
			{
				await AddTestUsers(services.GetRequiredService<RoleManager<Entities.UserRole>>(), services.GetRequiredService<UserManager<Entities.User>>());
			}
		}

		//public static async Task AddTestDataHotel(HotelApiDbContext _context)

		//{
		//	if (_context.Rooms.Any())
		//	{
		//		return;
		//	}

		//	_context.Rooms.Add(new Models.RoomEntity
		//	{
		//		ID = Guid.Parse("2620ba2e-3cae-4b01-9f48-b9f25ac1a42a"),
		//		Name = "Oxford Suite",
		//		Rate = 10119
		//	});
		//	_context.Rooms.Add(new Models.RoomEntity
		//	{
		//		ID = Guid.Parse("2251ba2e-3cae-4b01-9f48-b9f25ac1a47a"),
		//		Name = "Driscll Suite",
		//		Rate = 23959
		//	}
		//	);


		//	await _context.SaveChangesAsync();
		//}

	

		private static async Task AddTestUsers( RoleManager<Entities.UserRole> roleManager, UserManager<Entities.User> userManager)
		{
			var dataExists = roleManager.Roles.Any() || userManager.Users.Any();
			if (dataExists)
				return;

			await roleManager.CreateAsync(new Entities.UserRole("Admin"));

			var user = new Entities.User
			{
				Forename = "Admin",
				UserName = "admin"
			};

			await userManager.CreateAsync(user, "adminPassword");

			//put the user in the role

			await userManager.AddToRoleAsync(user, "Admin");
			await userManager.UpdateAsync(user);
		}
	}
}
