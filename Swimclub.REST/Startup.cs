using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Swimclub;
using System.IO;
using Microsoft.Data.Sqlite;
using System.Reflection;
using OpenIddict.Validation;
using AspNet.Security.OpenIdConnect.Primitives;

namespace Swimclub.REST
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			CheckDatabases();
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{

			string data_path = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "data");
			var locationOfUsers = String.Format("Data Source={0}", Path.Combine(data_path, "users.db3"));
			var userConnectionString = new SqliteConnectionStringBuilder(locationOfUsers)
			{
				Mode = SqliteOpenMode.ReadWriteCreate,
				Password = "temp"   //FIGURE OUT A SAFER WAY FOR THIS IMMEDIATELY
			}.ToString();


			services.AddDbContext<Data.UserDbContext>(options => { 
				options.UseSqlite(userConnectionString);
				options.UseOpenIddict<int>();
			});

			services.AddIdentity<Entities.User, Entities.UserRole>(options =>
			{
				options.Password.RequireDigit = true;
				options.Password.RequireLowercase = true;
				options.Password.RequiredLength = 5;
			}
			).AddEntityFrameworkStores<Data.UserDbContext>().AddDefaultTokenProviders();




			services.AddMvc(options =>
			{
				options.Filters.Add<Filters.JsonExceptionFilter>();
				options.Filters.Add<Filters.RequireHttpsOrClose>();
			}
			);

			services.AddControllers();
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "Swimclub.REST", Version = "v1" });
			});

			services.AddRouting(options => options.LowercaseUrls = true);
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseSwagger();
				app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Swimclub.REST v1"));
			}


			app.UseRouting();

			app.UseAuthentication();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}

		private void CheckDatabases()
		{
			//DB files
			//users
			//resources
			string data_path = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "data");
			Directory.CreateDirectory(data_path);
			var locationOfUsers = String.Format("Data Source={0}", Path.Combine(data_path, "users.db3"));
			var userConnectionString = new SqliteConnectionStringBuilder(locationOfUsers)
			{
				Mode = SqliteOpenMode.ReadWriteCreate,
				Password = "temp"	//FIGURE OUT A SAFER WAY FOR THIS IMMEDIATELY
			}.ToString();

			SqliteConnection userCon = new SqliteConnection(userConnectionString);
			userCon.Open();
			SqliteCommand cmd = userCon.CreateCommand();
			cmd.CommandText = "SELECT count(*) FROM sqlite_master";
			Int64 ret = (Int64)cmd.ExecuteScalar();
			if(ret == 0)
			{
				//Empty. Proceed to seed that database with data? maybe? Set a flag for seeding?
				LaunchFlags.Instance.NewHost = true;
				userCon.Close();
				File.Delete(Path.Combine(data_path, "users.db3"));

			} else
			{
				LaunchFlags.Instance.NewHost = false;
				userCon.Close();
			}

		}

		private static void AddIdentityCoreServices(IServiceCollection services)
		{
			var builder = services.AddIdentityCore<Entities.User>();

			builder = new IdentityBuilder(
				builder.UserType,
				typeof(Entities.UserRole),
				builder.Services);

			builder.AddRoles<Entities.UserRole>()
				.AddEntityFrameworkStores<Data.UserDbContext>()
				.AddDefaultTokenProviders()
				.AddSignInManager<SignInManager<Entities.User>>();
		}
	}
}
