using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Swimclub;
using Swimclub.Models;

namespace Swimclub.REST.Filters
{
	public class JsonExceptionFilter : IExceptionFilter
	{
		private readonly IHostingEnvironment env;
		public JsonExceptionFilter(IHostingEnvironment _env)
		{
			env = _env;
		}

		public void OnException(ExceptionContext context)
		{
			var error = new ApiError();
			if (env.IsDevelopment())
			{
				error.Success = false;
				error.Message = context.Exception.Message;
				error.Detail = context.Exception.StackTrace;
			}
			else
			{
				error.Success = false;
				error.Message = "Server error occurred.";
				error.Detail = context.Exception.Message;
			}
			

			context.Result = new ObjectResult(error)
			{
				StatusCode = 500
			};
		}
	}
}
