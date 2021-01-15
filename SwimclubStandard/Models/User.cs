using System;
using System.Collections.Generic;
using System.Text;

namespace Swimclub.Models
{
	//[Table("Users")]
	//public class User : Iden<int>
	//{
	//	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	//	public int ID { get; set; }
	//	public string Forename { get; set; }
	//	public string Username { get; set; }
	//}

	public class User
	{
		public int ID { get; set; }
		public string Username { get; set; }
		public string Forename { get; set; }
		public string Surname { get; set; }
	}

}
