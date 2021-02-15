using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Swimclub.REST.Entities
{
	public class UserRole : IdentityRole<int>
	{
		public UserRole() : base()
		{

		}

		public UserRole(string roleName) : base(roleName)
		{

		}

	}
}
