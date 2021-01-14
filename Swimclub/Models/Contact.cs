using System;
using System.Collections.Generic;
using System.Text;

namespace Swimclub.Models
{
	public class Contact
	{
		public string FullName { get; set; }
		public string PhoneNumber { get; set; }
		public string MobileNumber { get; set; }
		public Address Address { get; set; }
	}
}
