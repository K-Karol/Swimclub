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

			_context.Students.Add(new Entities.Student() // each student needs
			{
				Forename = "Joe", //first name
				Surname = "Bloggs", //last name
				CurrentGradeNumber = 1, //default starting number
				SwimEnglandNumber = "12345678", //number given by goverment
				DateOfBirth = new DateTime(2008, 1, 1), //When they were born
				MedicalDetails = new Models.MedicalDetails() // medical details and emergeny contact
				{
					Allergies = new string[] { "Strawberries" }, //if (and what) algeries the child has, only inculde if one exists
					Notes = "Struggles with endurance", // notes about student (medical problems)
					Immunizations = new string[] { "Teenage booster?" }, // recient vaccines taken
					Illnesses = new string[] {"Mumps"}, // curent illness' possesed
					EmergencyContacts = new Models.Contact[] // who and where is thir emergency contact
					{
						new Models.Contact() 
						{ 
							FullName = "Sarah Bloggs", MobileNumber = "07777777777", Address = new Models.Address() // name, mobile number, address and postcode of the emergency contact
							{
								Line1 = "1 Awesome Street", Line2 = "Eckington", Line3 = "Sheffield", Line4 = "Yorkshire", PostCode = "S11 2AB"
							}
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
					EmergencyContacts = new Models.Contact[] 
					{
						new Models.Contact() 
						{ 
							FullName = "Sarah Bloggs", MobileNumber = "07777777777", Address = new Models.Address()
							{
								Line1 = "1 Awesome Street", Line2 = "Eckington", Line3 = "Sheffield", Line4 = "Yorkshire", PostCode = "S11 2AB"
							}
						}
					}
				}
			});

			_context.Students.Add(new Entities.Student() 
			{ 
				Forename = "John",
				Surname = "Lennon",
				CurrentGradeNumber = 1,
				SwimEnglandNumber = "29473029",
				DateOfBirth = new DateTime(2008, 12, 12),
				MedicalDetails = new Models.MedicalDetails()
                {
					Illnesses = new string[] { "BlackLung", "ColiflowerEar"},
					EmergencyContacts = new Models.Contact[]
                    {
						new Models.Contact()
                        {
							FullName = "Henry Goafer", MobileNumber = "07999888777", Address = new Models.Address()
                            {
								Line1 = "23 Waliby Way", Line2 = "PortTon", Line3="New Bristol", Line4="Sidney", PostCode="FT16 9BC"
                            }
                        }
                    }
                }

			});

			_context.Students.Add(new Entities.Student()
			{
				Forename = "Jessie",
				Surname = "Hampton",
				CurrentGradeNumber = 1,
				SwimEnglandNumber = "50378920", 
				DateOfBirth = new DateTime(2009, 01, 29),
				MedicalDetails = new Models.MedicalDetails()
				{
					Allergies = new string[] { "Latex" },
					EmergencyContacts = new Models.Contact[]
					{
						new Models.Contact()
						{
							FullName = "Jenry Hover", MobileNumber = "07989878767", Address = new Models.Address()
							{
								Line1 = "18 Dusty Road", Line2 = "ButAsh", Line3="HampShire", Line4 = "England", PostCode="FT16 9BC"
							}
						}
					}
				}

			});

			_context.Students.Add(new Entities.Student()
			{
				Forename = "Olivier",
				Surname = "Isfruit",
				CurrentGradeNumber = 2,
				SwimEnglandNumber = "02987831",
				DateOfBirth = new DateTime(2007, 10, 31),
				MedicalDetails = new Models.MedicalDetails()
				{
					EmergencyContacts = new Models.Contact[]
					{
						new Models.Contact()
						{
							FullName = "Tomateo Isfruit", MobileNumber = "09850729864", Address = new Models.Address()
							{
								Line1 = "1 Green House", Line2 = "Byline", Line3="Glostershire", Line4="England", PostCode="GY29 H43"
							}
						}
					}
				}

			});

			_context.Students.Add(new Entities.Student()
			{
				Forename = "Crystal",
				Surname = "Ball",
				CurrentGradeNumber = 2,
				SwimEnglandNumber = "09284375",
				DateOfBirth = new DateTime(2007, 08, 04),
				MedicalDetails = new Models.MedicalDetails()
				{
					Allergies = new string[] { "Aquagenic urticaria", "Xeroderma Pigmentosum" },
					Illnesses = new string[] { "Acute lymphoblastic leukaemia" },
					EmergencyContacts = new Models.Contact[]
					{
						new Models.Contact()
						{
							FullName = "Dom Ball", MobileNumber = "07927927864", Address = new Models.Address()
							{
								Line1 = "36 Places Street", Line2 = "Somewhere", Line3="Deven", Line4="England", PostCode="GY29 H43"
							}
						}
					}
				}

			});


			await _context.SaveChangesAsync();

		}
	}
}
