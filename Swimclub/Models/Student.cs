using System;
using System.Collections.Generic;
using System.Text;

namespace Swimclub.Models
{
	public class Student
	{
		public int ID { get; set; }
		public string Forename { get; set; }
		public string Surname { get; set; }
		public string SwimEnglandNumber { get; set; }
		public DateTime DateOfBirth { get; set; }
		public int CurrentGradeNumber { get; set; }
		public MedicalDetails MedicalDetails { get; set; }
	}
}
