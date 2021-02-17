using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Swimclub.Models
{
	public class ApiError
	{
		[JsonIgnore]
		public bool Success { get; set; }
		public string Message { get; set; }
		public string Detail { get; set; }
	}
	public class UserInfoResponse
	{
		public string username { get; set; }
		public string forename { get; set; }
		public string surname { get; set; }
		public Models.ApiError error { get; set; }

	}
	public class AuthResponse
	{
		public bool Success { get; set; }
		public string Token { get; set; }
		public DateTime? Expiry { get; set; }
		public Models.ApiError Error { get; set; }
		public string Role { get; set; }
	}
	public class UserServiceResponse
	{
		public bool success;
		public string token { get; set; }
		public DateTime? ExpireDate { get; set; }
		public string Role { get; set; }
	}
	public class AllStudentsResponse
	{
		public bool Success { get; set; }
		public Models.ApiError Error { get; set; }
		public standard.Collection<Models.Student> Students { get; set; }
	}

	public class ModifyStudentResponse
	{
		public bool Success { get; set; }
		public Models.ApiError Error { get; set; }
		public standard.Item<Models.Student> Student { get; set; }
	}

	public class SingularStudentResponse
	{
		public bool Success { get; set; }
		public Models.ApiError Error { get; set; }
		public standard.Item<Models.Student> Student { get; set; }
	}

	public class CreateStudentResponse
	{
		public bool Success { get; set; }
		public Models.ApiError Error { get; set; }
		public standard.Item<Models.Student> Student { get; set; }
	}

	public class DeleteStudentResponse
	{
		public bool Success { get; set; }
		public Models.ApiError Error { get; set; }
		public int ID { get; set; }
	}

	public class AllGradeResponse
	{
		public bool Success { get; set; }
		public Models.ApiError Error { get; set; }
		public standard.Collection<Models.Grade> Grades { get; set; }
	}

	

	namespace standard
	{
		public class Collection<T>
		{
			public bool success { get; set; }
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
			public bool success { get; set; }
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
