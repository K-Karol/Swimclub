using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Swimclub.REST.Data
{
	public class UserDbContext : IdentityDbContext<Entities.User, Entities.UserRole, int>
	{
		public UserDbContext(DbContextOptions<UserDbContext> _options) : base(_options) { Database.EnsureCreated();  }

		//public DbSet<Entities.User> Users { get; set; }
	}
}
