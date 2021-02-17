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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace Swimclub.REST
{
	public class Startup
	{

		private string userConnectionString;
		private string studentConnectionString;
		private string resourceConnectionString;

		public Startup(IConfiguration configuration)
		{
			
			Configuration = configuration;
			ConfigureDatabases();
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{

			services.AddDbContext<Data.UserDbContext>(options => { 
				options.UseSqlite(userConnectionString);
			});

			services.AddDbContext<Data.StudentDbContext>(options => {
				options.UseSqlite(studentConnectionString);
			});

			services.AddDbContext<Data.ResourceDbContext>(options => {
				options.UseSqlite(resourceConnectionString);
			});

			AddIdentityCoreServices(services);


			services.AddAuthentication(auth =>
			{
				auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			}
			).AddJwtBearer(options =>
			{
				options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidAudience = Configuration["AuthSettings:Audience"],
					ValidIssuer = Configuration["AuthSettings:Issuer"],
					RequireExpirationTime = true,
					IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(Configuration["AuthSettings:Key"])),
					ValidateIssuerSigningKey = true
				};
			
			});

			//services.Configure<IdentityOptions>(options => {
			//	options.ClaimsIdentity.UserNameClaimType = OpenIdConnectConstants.Claims.Name;
			//	options.ClaimsIdentity.UserIdClaimType = OpenIdConnectConstants.Claims.Subject;
			//	options.ClaimsIdentity.RoleClaimType = OpenIdConnectConstants.Claims.Role;
			//});

			services.AddAuthorization(opt =>
			{
				opt.AddPolicy("AdminPolicy", p => p.RequireAuthenticatedUser().RequireClaim(ClaimTypes.Role,"Admin")) ;
				opt.AddPolicy("CoachPolicy", p => p.RequireAuthenticatedUser().RequireRole(ClaimTypes.Role, "Admin", "Coach"));
			});

			services.AddMvc(options =>
			{
				options.Filters.Add<Filters.JsonExceptionFilter>();
				options.Filters.Add<Filters.RequireHttpsOrClose>();
			}
			).ConfigureApiBehaviorOptions(x=>x.SuppressMapClientErrors = true);

			services.AddScoped<Services.IUserService, Services.UserService>();

			services.AddControllers();
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "Swimclub.REST", Version = "v1" });
				c.IncludeXmlComments(string.Format(@"{0}\bin\SwaggerAPI.xml", Directory.GetCurrentDirectory()));
				c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
				{
					Type = SecuritySchemeType.Http,
					BearerFormat = "JWT",
					In = ParameterLocation.Header,
					Scheme = "Bearer"

				});
				c.OperationFilter<Filters.AuthenticationRequirementsOperationFilter>();

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

		private void ConfigureDatabases()
		{
			string data_path = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "data");
			Directory.CreateDirectory(data_path);
			var locationOfUsers = String.Format("Data Source={0}", Path.Combine(data_path, "users.db3"));
			userConnectionString = new SqliteConnectionStringBuilder(locationOfUsers)
			{
				Mode = SqliteOpenMode.ReadWriteCreate,
				Password = Configuration["DatabaseSettings:UserPassword"]
			}.ToString();

			var locationOfStudents = String.Format("Data Source={0}", Path.Combine(data_path, "students.db3"));
			studentConnectionString = new SqliteConnectionStringBuilder(locationOfStudents)
			{
				Mode = SqliteOpenMode.ReadWriteCreate,
				Password = Configuration["DatabaseSettings:StudentPassword"]
			}.ToString();

			var locationOfResources = String.Format("Data Source={0}", Path.Combine(data_path, "resources.db3"));
			resourceConnectionString = new SqliteConnectionStringBuilder(locationOfResources)
			{
				Mode = SqliteOpenMode.ReadWriteCreate,
				Password = Configuration["DatabaseSettings:ResourcePassword"]
			}.ToString();

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
