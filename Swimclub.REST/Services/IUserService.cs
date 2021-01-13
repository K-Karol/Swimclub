using Microsoft.AspNetCore.Identity;
using Swimclub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Swimclub.REST.Services
{
	public interface IUserService
	{

		Task<Models.ApiError> LoginUserAsync(Models.Login _model);

	}

	public class UserService : IUserService
	{
		private UserManager<Entities.User> user_manager;

		public UserService(UserManager<Entities.User> _user_manager)
		{
			user_manager = _user_manager;
		}

		public async Task<ApiError> LoginUserAsync(Login _model)
		{
			var user = await user_manager.FindByNameAsync(_model.username);

			var error = new ApiError();
			error.Success = false;
			error.Message = "Incorrect username or password";
			error.Detail = "Please try again";

			if (user == null)
				return error;
			else
			{
				var passwordCheck = await user_manager.CheckPasswordAsync(user, _model.password);
				if (!passwordCheck)
					return error;

				var success = new ApiError();
				success.Success = true;
				success.Message = "Login succesful";
				success.Detail = "You may now use the API";
				return success;
			}
		}
	}
}
