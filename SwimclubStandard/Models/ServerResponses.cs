using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Swimclub.Models
{
	public class ApiError
	{
		public string Message { get; set; }
		public string Detail { get; set; }
		public int Code { get; set; }

		public static ApiError UnAuthResponse()
		{
			return new Models.ApiError() { Message = "Unauthorised request", Detail = "This user is not authorised to perform this action", Code = (int)Models.ServerResponse.ErrorCodes.UNAUTHORISED };
		}

		public static ApiError TimeOutResponse()
		{
			return new Models.ApiError() { Message = "Timeout", Detail = "Either your connection is offline or the server is currently down for maintenance", Code = (int)Models.ServerResponse.ErrorCodes.TIME_OUT };
		}

		public static ApiError NotLoggedInError()
		{
			return new Models.ApiError() { Message = "You are not logged in", Detail = "You are not logged in", Code = (int)Models.ServerResponse.ErrorCodes.NOT_LOGGEDIN };
		}
	}


	public abstract class ServerResponse
	{
		public bool Success { get; set; }
		public ApiError Error { get; set; }
		public enum ErrorCodes { MISC = 0, USER_NOT_EXIST =  1, USER_CREDENTIALS_INCORRECT = 2, PASSWORD_INVALID = 3, REGISTRATION_ERROR = 4, UNAUTHORISED = 5, STUDENT_NOT_EXIST = 6, MISSING_DATA = 7, TIME_OUT = 8, NOT_LOGGEDIN = 9}
	}


	public class UserInfoResponse : ServerResponse
	{
		public Models.User User { get; set; }
	}

	public class AllUsersResponse : ServerResponse
	{
		public standard.Collection<Models.User> Users { get; set; }
	}

	public class AuthResponse : ServerResponse
	{
		public string Token { get; set; }
		public DateTime? Expiry { get; set; }
		public string Role { get; set; }
	}

	public class RegistrationResponse : ServerResponse
	{
		public standard.Collection<string> PasswordValidationErrors { get; set; }
	}

	public class AllStudentsResponse : ServerResponse
	{
		public standard.Collection<Models.Student> Students { get; set; }
	}
	
	public class ModifyStudentResponse : ServerResponse
	{
		public standard.Item<Models.Student> Student { get; set; }
	}

	public class StudentResponse : ServerResponse
	{
		public standard.Item<Models.Student> Student { get; set; }
	}

	public class CreateStudentResponse : ServerResponse
	{
		public standard.Item<Models.Student> Student { get; set; }
	}

	public class DeleteStudentResponse : ServerResponse
	{
		public int ID { get; set; }
	}

	public class AllGradesResponse : ServerResponse
	{
		public standard.Collection<Models.Grade> Grades { get; set; }
	}

	public class StudentGradeTestsResponse : ServerResponse
	{
		public standard.Item<Models.StudentGradeTests> StudentGradeTests { get; set; }
	}

	public class ModifyStudentGradeTestResponse : ServerResponse
	{
		public bool AdvancedGrade { get; set; }
		public bool MaxGrade { get; set; }
		public standard.Item<Models.Student> Student { get; set; }
	}


	namespace standard
	{
		public class Collection<T>
		{
			[JsonIgnore]
			public T[] values { get; set; }
			public int length { get; set; }
			public string valuesjson
			{
				get { return JsonConvert.SerializeObject(values); }
				set { values = JsonConvert.DeserializeObject<T[]>(value); }
			}
		}

		public class Item<T>
		{
			[JsonIgnore]
			public T itemValue { get; set; }
			public string valuejson
			{
				get { return JsonConvert.SerializeObject(itemValue); }
				set { itemValue = JsonConvert.DeserializeObject<T>(value); }
			}
		}
	}
	
}
