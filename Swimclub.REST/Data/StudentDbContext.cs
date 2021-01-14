using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Swimclub.REST.Data
{
	public class StudentDbContext : DbContext
	{
		public StudentDbContext(DbContextOptions<StudentDbContext> _options) : base(_options) { Database.EnsureCreated(); }

		public DbSet<Entities.Student> Students { get; set; }
	}
}
