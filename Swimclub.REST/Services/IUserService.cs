using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Swimclub.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Swimclub.REST.Services
{
	public interface IUserService
	{

		Task<Models.UserServiceResponse> LoginUserAsync(Models.Login _model);
		Task<int?> GetUserIdAsync(ClaimsPrincipal principal);

		Task<User> GetUserAsync(ClaimsPrincipal user);
	}

	public class UserService : IUserService
	{
		

		private UserManager<Entities.User> user_manager;
		private IConfiguration configuration;

		public UserService(UserManager<Entities.User> _user_manager, IConfiguration _configuration)
		{
			user_manager = _user_manager;
			configuration = _configuration;
		}

		public async Task<Models.UserServiceResponse> LoginUserAsync(Login _model)
		{
			var user = await user_manager.FindByNameAsync(_model.username);

			if (user == null)
				return new Models.UserServiceResponse() { success = false };
			else
			{
				var passwordCheck = await user_manager.CheckPasswordAsync(user, _model.password);
				if (!passwordCheck)
					return new Models.UserServiceResponse() { success = false };

				var getRoles = await user_manager.GetRolesAsync(user);

				var claims = new[]
				{
					new Claim("Username",user.UserName),
					new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
					new Claim(ClaimTypes.Role, getRoles[0])
				};

				var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["AuthSettings:Key"]));
				var token = new JwtSecurityToken(issuer: configuration["AuthSettings:Issuer"], audience: configuration["AuthSettings:Audience"], claims: claims, expires: DateTime.Now.AddHours(8),
					signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

				string tokenAsString = new JwtSecurityTokenHandler().WriteToken(token);

				

				string role = "NA";
				if(getRoles.Count >=1)
				{
					role = getRoles[0];
				}

				return new Models.UserServiceResponse() { success = true, token = tokenAsString, ExpireDate = token.ValidTo, Role = role }; ;
			}
		}

		public async Task<User> GetUserAsync(ClaimsPrincipal user)
		{
			var entity = await user_manager.GetUserAsync(user);
			return Entities.User.getUser(entity);
		}

		public async Task<int?> GetUserIdAsync(ClaimsPrincipal principal)
		{
			var user = await user_manager.GetUserAsync(principal);
			if (user == null) return null;

			return user.Id;
		}


	}
}
