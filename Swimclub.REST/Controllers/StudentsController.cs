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
	public class StudentsController : ControllerBase
	{
		private readonly Data.StudentDbContext studentDbContext;

		public StudentsController(Data.StudentDbContext _context)
		{
			studentDbContext = _context;
		}
		[HttpGet("all",Name =nameof(GetAllStudents))]
		public async Task<ActionResult<Models.Collection<Models.Student>>> GetAllStudents()
		{
			Entities.Student[] entities = await studentDbContext.Students.ToArrayAsync();
			Models.Student[] students = entities.Select(ent => Entities.Student.GetStudent(ent)).ToArray();
			Models.Collection<Models.Student> ret = new Models.Collection<Models.Student>() { success = true, values = students, length = students.Length };
			return Ok(ret);
		}
	}
}
