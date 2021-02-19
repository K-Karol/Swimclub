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
	public class ClassController : ControllerBase
	{
		private readonly Data.StudentDbContext studentDbContext;
		private readonly Data.ResourceDbContext resourceDbContext;
		private readonly Data.UserDbContext userDbContext;
		private readonly IAuthorizationService authService;
		public ClassController(Data.StudentDbContext _studentContext, Data.ResourceDbContext _resourceDbContext, Data.UserDbContext _userDb,
			IAuthorizationService _authService)
		{
			studentDbContext = _studentContext;
			resourceDbContext = _resourceDbContext;
			userDbContext = _userDb;
			authService = _authService;
		}




		[HttpPost("add", Name = nameof(AddClass))]
		public async Task<ActionResult<Models.AddClassResponse>> AddClass([FromBody] Models.Class _class)
		{
			Models.AddClassResponse response;

			if (User.Identity.IsAuthenticated)
			{
				var policyCheck = await authService.AuthorizeAsync(User, "CoachPolicy");
				if (!policyCheck.Succeeded)
				{
					return Unauthorized(response = new Models.AddClassResponse() { Success = false, Error = Models.ApiError.UnAuthResponse() });

				}
			}


			Entities.Class classEnt = new Entities.Class() { ClassGrade = _class.ClassGrade, coachID = _class.coach.ID, Pool = _class.Pool, TimeOfClass = _class.TimeOfClass};
			int[] StudentIDs = _class.Students.Select(c => c.ID).ToArray();
			List<bool> tempAttendance = new List<bool>();
			foreach(int i in StudentIDs)
			{
				tempAttendance.Add(_class.Attendance[i]);
			}
			bool[] Attendance = tempAttendance.ToArray();
			classEnt.StudentAttendance = Attendance;
			classEnt.StudentIDs = StudentIDs;
			await resourceDbContext.AddAsync(classEnt);
			await resourceDbContext.SaveChangesAsync();
			_class.ID = classEnt.ID;
			_class.ClassGrade = classEnt.ClassGrade;
			response = new Models.AddClassResponse() { Success = true, Class = new Models.standard.Item<Models.Class>() { itemValue = _class } };
			return Ok(response);
		}

		[HttpGet("all", Name = nameof(AllClasses))]
		public async Task<ActionResult<Models.AllClassResponse>> AllClasses()
		{
			Models.AllClassResponse response;

			if (User.Identity.IsAuthenticated)
			{
				var policyCheck = await authService.AuthorizeAsync(User, "CoachPolicy");
				if (!policyCheck.Succeeded)
				{
					return Unauthorized(response = new Models.AllClassResponse() { Success = false, Error = Models.ApiError.UnAuthResponse() });

				}
			}


			Entities.Class[] classesEntities = await resourceDbContext.Classes.ToArrayAsync();
			Entities.Student[] studentEntities = await studentDbContext.Students.ToArrayAsync();
			Entities.Grade[] gradeEntities = await resourceDbContext.Grades.ToArrayAsync();
			Entities.User[] userEntities = await userDbContext.Users.ToArrayAsync();
			List<Models.Class> classModels = new List<Models.Class>();
			foreach(Entities.Class ent in classesEntities)
			{
				Models.Class temp = new Models.Class() { ID = ent.ID, TimeOfClass = ent.TimeOfClass, ClassGrade = ent.ClassGrade};
				Entities.User tempUserent = userEntities.FirstOrDefault<Entities.User>(e => e.Id == ent.coachID);
				if(tempUserent == null) return BadRequest(new Models.AllClassResponse() { Success = false, Error = new Models.ApiError() { Message = "Coudln't fetch a class", Detail = "This coach does not exist", Code = (int)Models.ServerResponse.ErrorCodes.MISSING_DATA } });

				Models.User tempuser = Entities.User.getUser(tempUserent);
				temp.coach = tempuser;
				List<Models.Student> tempSList = new List<Models.Student>();
				Dictionary<int, bool> tempAtt = new Dictionary<int, bool>();

				for(int i = 0; i < ent.StudentIDs.Length; i++)
				{
					int id = ent.StudentIDs[i];
					Entities.Student sTempEnt = studentEntities.FirstOrDefault(s => s.ID == id);
					if (sTempEnt == null) return BadRequest(new Models.AllClassResponse() { Success = false, Error = new Models.ApiError() { Message = "Coudln't fetch a class", Detail = "This student does not exist", Code = (int)Models.ServerResponse.ErrorCodes.STUDENT_NOT_EXIST } });
					Models.Student sMod = Entities.Student.GetStudent(sTempEnt);
					tempSList.Add(sMod);
					tempAtt.Add(sMod.ID, ent.StudentAttendance[i]);

				}

				temp.Students = tempSList.ToArray();
				temp.Attendance = tempAtt;

				classModels.Add(temp);
			}

			response = new Models.AllClassResponse() { Success = true, Classes = new Models.standard.Collection<Models.Class>() { length = classModels.Count, values = classModels.ToArray() } };

			return Ok(response);
		}
	}
}
