using System;
using System.Collections.Generic;
using System.Text;

namespace Swimclub.Models
{
	public class Room : Resource
	{
		public string Name { get; set; }

		public decimal Rate { get; set; }
	}

	public class RoomEntity
	{
		public Guid ID { get; set; }

		public string Name { get; set; }

		public int Rate { get; set; }
	}
}
