using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Swimclub.REST.Data
{
	public class ResourceDbContext : DbContext
	{
		public ResourceDbContext(DbContextOptions<ResourceDbContext> _options) : base(_options) { Database.EnsureCreated(); }

		public DbSet<Entities.Grade> Grades { get; set; }
		public DbSet<Entities.StudentGradeTests> StudentGradeTests { get; set; }
	}
}
