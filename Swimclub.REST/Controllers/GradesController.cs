using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Swimclub.REST.Controllers
{
	[Authorize]
	[ApiController]
	[Route("/[controller]")]
	public class GradesController : ControllerBase
	{
		private readonly Data.StudentDbContext studentDbContext;
		private readonly Data.ResourceDbContext resourceDbContext;
		private readonly IAuthorizationService authService;
		public GradesController(Data.StudentDbContext _studentContext, Data.ResourceDbContext _resourceDbContext,
			IAuthorizationService _authService)
		{
			studentDbContext = _studentContext;
			resourceDbContext = _resourceDbContext;
			authService = _authService;
		}

		[HttpGet("all", Name = nameof(GetAllGrades))]
		public async Task<ActionResult<Models.AllStudentsResponse>> GetAllGrades()
		{
			Models.AllGradeResponse response;

			if (User.Identity.IsAuthenticated){
				var policyCheck = await authService.AuthorizeAsync(User, "CoachPolicy");
				if (!policyCheck.Succeeded)
				{
					return Unauthorized(response = new Models.AllGradeResponse() { Success = false, Error = new Models.ApiError() { Success = false, Message = "This user is not authorised.", Detail = "Requirements: Admin or Coach" } });

				}
			}

			Entities.Student[] studentEntities = await studentDbContext.Students.ToArrayAsync();
			Models.Student[] students = studentEntities.Select(ent => Entities.Student.GetStudent(ent)).ToArray();

			Entities.Grade[] gradeEntities = await resourceDbContext.Grades.ToArrayAsync();
			List<Models.Grade> grades = new List<Models.Grade>();
			foreach (var item in gradeEntities)
			{
				Models.Grade newGrade = new Models.Grade() { ID = item.ID, Number = item.Number, Tests = item.Tests };
				Models.Student[] studentsInThisGrade = students.Where(s => s.CurrentGradeNumber == newGrade.Number).ToArray();
				newGrade.Students = studentsInThisGrade;
				grades.Add(newGrade);
			}

			Models.standard.Collection<Models.Grade> ret = new Models.standard.Collection<Models.Grade>() { success = true, values = grades.ToArray(), length = grades.Count };
			response = new Models.AllGradeResponse() { Success = true, Error = new Models.ApiError() { Success = true }, Grades = ret };
			return Ok(response);
		}
	}
}
