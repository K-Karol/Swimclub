using System;
using System.Collections.Generic;
using System.Text;

namespace Swimclub.Models
{
	public class StudentGradeTests
	{
		public int ID { get; set; }
		public Models.TestAttempt[] TestAttempts { get; set; }
	}

	public class StudentGradeTestRequest
	{
		public int ID { get; set; }
	}
}
