using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Swimclub.REST.Entities
{
	[Table("Users")]
	public class User : IdentityUser<int>
	{
		public string Forename { get; set; }
		public string Surname { get; set; }
		public static Models.User getUser(Entities.User _user)
		{
			return new Models.User() { Forename = _user.Forename, ID = _user.Id, Surname = _user.Surname, Username = _user.UserName };
		}
	}
}
