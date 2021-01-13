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
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ID { get; set; }
		public string Forename { get; set; }
		public string Username { get; set; }
		//[NotMapped]
		//public Models.User UserClass { get { return new Models.User { ID = this.ID, Forename = this.Forename, Username = this.Username }; } }
	}
}
