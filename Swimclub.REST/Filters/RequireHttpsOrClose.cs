﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Swimclub.REST.Filters
{
	public class RequireHttpsOrClose : RequireHttpsAttribute
	{
		protected override void HandleNonHttpsRequest(AuthorizationFilterContext filterContext)
		{
			filterContext.Result = new StatusCodeResult(400);
		}
	}
}
