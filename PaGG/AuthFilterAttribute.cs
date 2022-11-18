using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.VisualBasic;
using PaGG.Controllers;
using PaGG.Core;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace PaGG
{
	public class AuthFilterAttribute : Attribute, IAsyncActionFilter
	{
		public async Task OnActionExecutionAsync(ActionExecutingContext curContext, ActionExecutionDelegate next)
		{
			var response = curContext.HttpContext.Response;
			
			var context = await next();

			//var controller = context.Controller as AccountsController;
			//var list = controller.RouteData.Values.ToList().Select(kvp => $"{kvp.Key}={(string)kvp.Value}");
			//string appended = string.Join(',', list);

			response.Headers.Add("executed-filter", "True");

			//context.Result = new ObjectResult(new { x = "y", y = 26, z = 0.00001 });
		}
	}
}
