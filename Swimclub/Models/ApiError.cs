using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
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
}
