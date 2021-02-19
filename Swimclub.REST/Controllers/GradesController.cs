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
		public async Task<ActionResult<Models.AllGradesResponse>> GetAllGrades()
		{
			Models.AllGradesResponse response;

			if (User.Identity.IsAuthenticated){
				var policyCheck = await authService.AuthorizeAsync(User, "CoachPolicy");
				if (!policyCheck.Succeeded)
				{
					return Unauthorized(response = new Models.AllGradesResponse() { Success = false, Error = Models.ApiError.UnAuthResponse() });

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

			Models.standard.Collection<Models.Grade> ret = new Models.standard.Collection<Models.Grade>() {values = grades.ToArray(), length = grades.Count };
			response = new Models.AllGradesResponse() { Success = true, Grades = ret };
			return Ok(response);
		}
		[HttpPost("studentgradetest/id", Name = nameof(GetSGTestByID))]
		public async Task<ActionResult<Models.StudentGradeTestsResponse>> GetSGTestByID([FromBody] Models.StudentGradeTestRequest _req)
		{
			Models.StudentGradeTestsResponse response;

			if (User.Identity.IsAuthenticated)
			{
				var policyCheck = await authService.AuthorizeAsync(User, "CoachPolicy");
				if (!policyCheck.Succeeded)
				{
					return Unauthorized(response = new Models.StudentGradeTestsResponse() { Success = false, Error = Models.ApiError.UnAuthResponse() });

				}
			}

			Entities.Student entity = await studentDbContext.Students.FirstOrDefaultAsync<Entities.Student>(e => e.ID == _req.ID);
			if (entity == null) return BadRequest(response = new Models.StudentGradeTestsResponse() { Success = false, Error = new Models.ApiError() { Message = "Cannot retrieve this record", Detail = "This student does not exist", Code = (int)Models.ServerResponse.ErrorCodes.STUDENT_NOT_EXIST } });
			Entities.Grade grade = await resourceDbContext.Grades.FirstOrDefaultAsync<Entities.Grade>(g => g.Number == entity.CurrentGradeNumber);
			if(grade == null) return BadRequest(response = new Models.StudentGradeTestsResponse() { Success = false, Error = new Models.ApiError() { Message = "Cannot retrieve this record", Detail = "This grade does not exist", Code = (int)Models.ServerResponse.ErrorCodes.MISC } });
			Entities.StudentGradeTests sgt = await resourceDbContext.StudentGradeTests.FirstOrDefaultAsync<Entities.StudentGradeTests>(s => s.StudentID == entity.ID && s.GradeID == grade.ID);
			if(sgt == null)
			{
				sgt = new Entities.StudentGradeTests() { GradeID = grade.ID, StudentID = entity.ID};
				List<Models.TestAttempt> temp = new List<Models.TestAttempt>();
				foreach(Models.Test test in grade.Tests)
				{
					temp.Add(new Models.TestAttempt() { AttemptedTest = test, Completed = false, DateTimeCompleted = null, Passed = false });
				}
				sgt.TestAttempts = temp.ToArray();
				await resourceDbContext.StudentGradeTests.AddAsync(sgt);
				await resourceDbContext.SaveChangesAsync();
			}

			Models.StudentGradeTests resp = new Models.StudentGradeTests() { ID = sgt.ID, TestAttempts = sgt.TestAttempts };
			return Ok(new Models.StudentGradeTestsResponse() { Success = true, StudentGradeTests = new Models.standard.Item<Models.StudentGradeTests>() { itemValue = resp } });
		}

		[HttpPost("studentgradetest/modify", Name = nameof(ModifySGTest))]
		public async Task<ActionResult<Models.ModifyStudentGradeTestResponse>> ModifySGTest([FromBody] Models.StudentGradeTests sg)
		{
			Models.ModifyStudentGradeTestResponse response;

			if (User.Identity.IsAuthenticated)
			{
				var policyCheck = await authService.AuthorizeAsync(User, "CoachPolicy");
				if (!policyCheck.Succeeded)
				{
					return Unauthorized(response = new Models.ModifyStudentGradeTestResponse() { Success = false, Error = Models.ApiError.UnAuthResponse() });

				}
			}

			//Entities.Student entity = await studentDbContext.Students.FirstOrDefaultAsync<Entities.Student>(e => e.ID == sg.ID);
			Entities.StudentGradeTests ent = await resourceDbContext.StudentGradeTests.FirstOrDefaultAsync<Entities.StudentGradeTests>(e => e.ID == sg.ID);
			if (ent == null) return BadRequest(response = new Models.ModifyStudentGradeTestResponse() { Success = false, Error = new Models.ApiError() { Message = "Cannot modify this record", Detail = "This StudentGradeTest does not exist", Code = (int)Models.ServerResponse.ErrorCodes.MISC } });
			Entities.Student student = await studentDbContext.Students.FirstOrDefaultAsync<Entities.Student>(s => s.ID == ent.StudentID);
			if (student == null) return BadRequest(response = new Models.ModifyStudentGradeTestResponse() { Success = false, Error = new Models.ApiError() { Message = "Cannot modify this record", Detail = "This student does not exist", Code = (int)Models.ServerResponse.ErrorCodes.STUDENT_NOT_EXIST } });

			ent.TestAttempts = sg.TestAttempts;
			bool allDone = true;
			response = new Models.ModifyStudentGradeTestResponse();
			foreach(Models.TestAttempt ta in ent.TestAttempts)
			{
				allDone = allDone & (ta.Completed && ta.Passed);
			}
			if (allDone)
			{
				int newGrade = student.CurrentGradeNumber + 1;
				Entities.Grade grade = await resourceDbContext.Grades.FirstOrDefaultAsync<Entities.Grade>(g => g.Number == newGrade);
				if(grade == null)
				{
					response.MaxGrade = true;
					response.AdvancedGrade = true;
				}
				else
				{
					response.AdvancedGrade = true;
					student.CurrentGradeNumber++;
					await studentDbContext.SaveChangesAsync();
				}
			}

			await resourceDbContext.SaveChangesAsync();
			response.Success = true;
			response.Student = new Models.standard.Item<Models.Student>() { itemValue = Entities.Student.GetStudent(student) };
			return Ok(response);
		}


	}
}
