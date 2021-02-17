using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Swimclub.REST.Controllers
{
	/// <summary>
	/// This authorised endpoint can be used to return all students in the database.
	/// </summary>
	[Authorize]
	[ApiController]
	[Route("/[controller]")]
	public class StudentsController : ControllerBase
	{
		private readonly Data.StudentDbContext studentDbContext;
		private readonly IAuthorizationService authService;

		public StudentsController(Data.StudentDbContext _context, IAuthorizationService _authService)
		{
			studentDbContext = _context;
			authService = _authService;
		}
		[HttpGet("all",Name =nameof(GetAllStudents))]
		public async Task<ActionResult<Models.AllStudentsResponse>> GetAllStudents()
		{
			Models.AllStudentsResponse response;
			if (User.Identity.IsAuthenticated)
			{
				var policyCheck = await authService.AuthorizeAsync(User, "CoachPolicy");
				if (!policyCheck.Succeeded)
				{
					return Unauthorized(response = new Models.AllStudentsResponse() { Success = false, Error = new Models.ApiError() { Success = false, Message = "This user is not authorised.", Detail = "Requirements: Admin or Coach" } });

				}
			}

			Entities.Student[] entities = await studentDbContext.Students.ToArrayAsync();
			Models.Student[] students = entities.Select(ent => Entities.Student.GetStudent(ent)).ToArray();
			Models.standard.Collection<Models.Student> ret = new Models.standard.Collection<Models.Student>() { success = true, values = students, length = students.Length };
			response = new Models.AllStudentsResponse() { Success = true, Error = new Models.ApiError() { Success = true }, Students = ret };
			return Ok(response);
		}
	}
}
