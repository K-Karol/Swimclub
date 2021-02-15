using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Swimclub.REST.Demo
{
	/// <summary>
	/// This static class will proceed to seed entities into the databases for the purpose of testing
	/// </summary>
	public static class SeedData
	{
		/// <summary>
		/// This will initialise the services and proceed to see
		/// </summary>
		/// <param name="services"></param>
		/// <returns></returns>
		public static async Task InitialiseAsync(IServiceProvider services)
		{
			await AddTestUsers(services.GetRequiredService<RoleManager<Entities.UserRole>>(), services.GetRequiredService<UserManager<Entities.User>>());
			await AddTestStudents(services.GetRequiredService<Data.StudentDbContext>());
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
				UserName = "admin",
				
			};

			Task<IdentityResult> t = userManager.CreateAsync(user, "adminPassword123!");
			t.Wait();
			//put the user in the role

			await userManager.AddToRoleAsync(user, "Admin");
			await userManager.UpdateAsync(user);

		}

		private static async Task AddTestStudents(Data.StudentDbContext _context)
		{
			if (_context.Students.Any())
			{
				return;
			}

			_context.Students.Add(new Entities.Student()
			{
				Forename = "Joe",
				Surname = "Bloggs",
				CurrentGradeNumber = 1,
				SwimEnglandNumber = "12345678",
				DateOfBirth = new DateTime(2008, 1, 1),
				MedicalDetails = new Models.MedicalDetails()
				{
					Allergies = new string[] { "Strawberries" },
					Notes = "Struggles with endurance",
					Immunizations = new string[] { "Teenage booster?" },
					EmergencyContacts = new Models.Contact[] {new Models.Contact() { FullName = "Sarah Bloggs", MobileNumber = "07777777777", Address = new Models.Address()
					{
					Line1 = "1 Awesome Street", Line2 = "Eckington", Line3 = "Sheffield", Line4 = "Yorkshire", PostCode = "S11 2AB"}
					}
				}
				}
			}
				);


			_context.Students.Add(new Entities.Student()
			{
				Forename = "Micheal",
				Surname = "Phelps",
				CurrentGradeNumber = 1,
				SwimEnglandNumber = "87654321",
				DateOfBirth = new DateTime(2009, 5, 11),
				MedicalDetails = new Models.MedicalDetails()
				{
					Illnesses = new string[] { "Sickle Cell Disease" },
					EmergencyContacts = new Models.Contact[] {new Models.Contact() { FullName = "Sarah Bloggs", MobileNumber = "07777777777", Address = new Models.Address()
					{
					Line1 = "1 Awesome Street", Line2 = "Eckington", Line3 = "Sheffield", Line4 = "Yorkshire", PostCode = "S11 2AB"}
					}
				}
				}
			}
				);


			await _context.SaveChangesAsync();

		}
	}
}
