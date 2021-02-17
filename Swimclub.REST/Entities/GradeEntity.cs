using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;

namespace Swimclub.REST.Entities
{
	[Table("Grades")]
	public class Grade
	{
		[Key, Required]
		public int ID { get; set; }
		[Required]
		public int Number { get; set; }
		[NotMapped]
		public Models.Test[] Tests { get; set; }
		public string TestsJson
		{
			get { return JsonSerializer.Serialize(Tests); } 
			set { Tests = JsonSerializer.Deserialize<Models.Test[]>(value); }
		}

	}
}
