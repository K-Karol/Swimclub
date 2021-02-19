using System;
using System.Collections.Generic;
using System.Text;

namespace Swimclub.Models
{
	public class TestAttempt
	{
		public Models.Test AttemptedTest { get; set; }
		public bool Completed { get; set; }
		public bool Passed { get; set; }
		public DateTime? DateTimeCompleted { get; set; }
	}
}
