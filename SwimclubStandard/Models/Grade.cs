using System;
using System.Collections.Generic;
using System.Text;

namespace Swimclub.Models
{
	public class Grade
	{
		public int ID { get; set; }
		public int Number { get; set; }
		public Test[] Tests {get;set;}
	}
}
