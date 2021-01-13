using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Swimclub.Models
{
	public class Login
	{
		[Required]
		public string username { get; set; }
		[Required]
		public string password { get; set; }
	}
}
