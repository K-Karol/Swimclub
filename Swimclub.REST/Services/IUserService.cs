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

		Task<UserService.UserServiceResponse> LoginUserAsync(Models.Login _model);

	}

	public class UserService : IUserService
	{
		public class UserServiceResponse
		{
			public bool success;
			public string token { get; set; }
			public DateTime? ExpireDate { get; set; }
		}

		private UserManager<Entities.User> user_manager;
		private IConfiguration configuration;

		public UserService(UserManager<Entities.User> _user_manager, IConfiguration _configuration)
		{
			user_manager = _user_manager;
			configuration = _configuration;
		}

		public async Task<UserService.UserServiceResponse> LoginUserAsync(Login _model)
		{
			var user = await user_manager.FindByNameAsync(_model.username);

			if (user == null)
				return new UserService.UserServiceResponse() { success = false };
			else
			{
				var passwordCheck = await user_manager.CheckPasswordAsync(user, _model.password);
				if (!passwordCheck)
					return new UserService.UserServiceResponse() { success = false };

				var claims = new[]
				{
					new Claim("Username",user.UserName),
					new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
				};

				var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["AuthSettings:Key"]));
				var token = new JwtSecurityToken(issuer: configuration["AuthSettings:Issuer"], audience: configuration["AuthSettings:Audience"], claims: claims, expires: DateTime.Now.AddHours(8),
					signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

				string tokenAsString = new JwtSecurityTokenHandler().WriteToken(token);

				return new UserService.UserServiceResponse() { success = true, token = tokenAsString, ExpireDate = token.ValidTo }; ;
			}
		}
	}
}
