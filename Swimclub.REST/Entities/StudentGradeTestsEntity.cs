using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Swimclub.REST.Entities
{
	[Table("StudentGradeTests")]
	public class StudentGradeTests
	{
		[Key,Required]
		public int ID { get; set; }
		[Required]
		public int StudentID { get; set; }
		[Required]
		public int GradeID { get; set; }
		[Required, NotMapped]
		public Models.TestAttempt[] TestAttempts { get; set; }
		public string TestAttemptsJSON
		{
			get { return JsonSerializer.Serialize(TestAttempts); }
			set { TestAttempts = JsonSerializer.Deserialize<Models.TestAttempt[]>(value); }
		}

	}
}
