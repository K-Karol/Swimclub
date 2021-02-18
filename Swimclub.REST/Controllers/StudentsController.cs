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
					return Unauthorized(response = new Models.AllStudentsResponse() { Success = false, Error = Models.ApiError.UnAuthResponse() });

				}
			}

			Entities.Student[] entities = await studentDbContext.Students.ToArrayAsync();
			Models.Student[] students = entities.Select(ent => Entities.Student.GetStudent(ent)).ToArray();
			Models.standard.Collection<Models.Student> ret = new Models.standard.Collection<Models.Student>() {values = students, length = students.Length };
			response = new Models.AllStudentsResponse() { Success = true, Students = ret };
			return Ok(response);
		}
		[HttpPost("modify", Name = nameof(ModifyStudent))]
		public async Task<ActionResult<Models.ModifyStudentResponse>> ModifyStudent([FromBody] Models.Student _student)
		{
			Models.ModifyStudentResponse response;
			if (User.Identity.IsAuthenticated)
			{
				var policyCheck = await authService.AuthorizeAsync(User, "AdminPolicy");
				if (!policyCheck.Succeeded)
				{
					return Unauthorized(response = new Models.ModifyStudentResponse() { Success = false, Error = Models.ApiError.UnAuthResponse() });

				}
			}

			Entities.Student entity = await studentDbContext.Students.FirstOrDefaultAsync<Entities.Student>(e => e.ID == _student.ID);
			if (entity == null) return BadRequest(response = new Models.ModifyStudentResponse() { Success = false, Error = new Models.ApiError() { Message = "Cannot modify this student", Detail = "This student does not exist", Code = (int)Models.ServerResponse.ErrorCodes.STUDENT_NOT_EXIST} });
			Entities.Student.ApplyChanges(_student, ref entity);
			await studentDbContext.SaveChangesAsync();
			response = new Models.ModifyStudentResponse() { Success = true, Student = new Models.standard.Item<Models.Student>() { itemValue = Entities.Student.GetStudent(entity)} };
			return Ok(response);
		}

		[HttpPost("get/id", Name = nameof(GetStudentByID))]
		public async Task<ActionResult<Models.StudentResponse>> GetStudentByID([FromBody] int _id)
		{
			Models.StudentResponse response;
			if (User.Identity.IsAuthenticated)
			{
				var policyCheck = await authService.AuthorizeAsync(User, "CoachPolicy");
				if (!policyCheck.Succeeded)
				{
					return Unauthorized(response = new Models.StudentResponse() { Success = false, Error = Models.ApiError.UnAuthResponse() });

				}
			}

			Entities.Student entity = await studentDbContext.Students.FirstOrDefaultAsync<Entities.Student>(e => e.ID == _id);
			if (entity == null) return BadRequest(response = new Models.StudentResponse() { Success = false, Error = new Models.ApiError() { Message = "Cannot retrieve this student", Detail = "This student does not exist" , Code = (int)Models.ServerResponse.ErrorCodes.STUDENT_NOT_EXIST} });
			response = new Models.StudentResponse() { Success = true, Student = new Models.standard.Item<Models.Student>() { itemValue = Entities.Student.GetStudent(entity)} };
			return Ok(response);
		}

		[HttpPost("add", Name = nameof(CreateStudent))]
		public async Task<ActionResult<Models.CreateStudentResponse>> CreateStudent([FromBody] Models.Student _student)
		{
			Models.CreateStudentResponse response;
			if (User.Identity.IsAuthenticated)
			{
				var policyCheck = await authService.AuthorizeAsync(User, "AdminPolicy");
				if (!policyCheck.Succeeded)
				{
					return Unauthorized(response = new Models.CreateStudentResponse() { Success = false, Error = Models.ApiError.UnAuthResponse() });

				}
			}

			Entities.Student newStudentEntity = Entities.Student.GetEntity(_student);
			if(!Entities.Student.CheckRequirements(newStudentEntity))
				return BadRequest(response = new Models.CreateStudentResponse() { Success = false, Error = new Models.ApiError() {Message = "Cannot create this student", Detail = "This request is missing data", Code = (int)Models.ServerResponse.ErrorCodes.MISSING_DATA} });

			await studentDbContext.AddAsync(newStudentEntity);
			await studentDbContext.SaveChangesAsync();
			response = new Models.CreateStudentResponse() { Success = true,Student = new Models.standard.Item<Models.Student>() { itemValue = Entities.Student.GetStudent(newStudentEntity)} };

			return Ok(response);
		}

		[HttpPost("delete/id", Name = nameof(DeleteStudent))]
		public async Task<ActionResult<Models.DeleteStudentResponse>> DeleteStudent([FromBody] int _id)
		{
			Models.DeleteStudentResponse response;
			if (User.Identity.IsAuthenticated)
			{
				var policyCheck = await authService.AuthorizeAsync(User, "AdminPolicy");
				if (!policyCheck.Succeeded)
				{
					return Unauthorized(response = new Models.DeleteStudentResponse() { Success = false, Error = Models.ApiError.UnAuthResponse() });

				}
			}

			Entities.Student entity = await studentDbContext.Students.FirstOrDefaultAsync<Entities.Student>(e => e.ID == _id);
			if (entity == null) return BadRequest(response = new Models.DeleteStudentResponse() { Success = false, Error = new Models.ApiError() { Message = "Cannot delete this student", Detail = "This student does not exist", Code = (int)Models.ServerResponse.ErrorCodes.STUDENT_NOT_EXIST} });

			studentDbContext.Remove(entity);
			await studentDbContext.SaveChangesAsync();
			response = new Models.DeleteStudentResponse() { Success = true, ID = _id};

			return Ok(response);
		}


	}
}
