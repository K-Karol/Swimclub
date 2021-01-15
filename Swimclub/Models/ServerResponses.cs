using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Swimclub.Models
{

	public class Collection<T>
	{
		public bool success { get; set; }
		[JsonIgnore]
		public T[] values { get; set; }
		public int length { get; set; }
		public string valuesjson
		{
			get { return JsonSerializer.Serialize(values); }
			set { values = JsonSerializer.Deserialize<T[]>(value); }
		}
	}
}
