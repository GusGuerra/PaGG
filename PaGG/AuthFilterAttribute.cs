using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
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
	public class AuthFilterAttribute : Attribute, IAsyncActionFilter //ActionFilterAttribute
	{
		public async Task OnActionExecutionAsync(ActionExecutingContext curContext, ActionExecutionDelegate next)
		{
			var response = curContext.HttpContext.Response;
			var request = curContext.HttpContext.Request;
			var cookieCollection = request.Cookies;

			// decode NÃO É NECESSÁRIO
			foreach (var cookie in cookieCollection)
            {
                if (!cookie.Value.Contains('='))
				{
					curContext.Result = new StatusCodeResult(StatusCodes.Status500InternalServerError);
					return;
				}
			}

			_ = await next();

			// workaround para setar cookie SEM ENCODING
			//response.Cookies.Append("name", "x=y");
			var setCookieHeaderValue = new SetCookieHeaderValue("name", "x=y")
			{
				Domain = request.Host.Host
			};

			var headers = response.Headers;
			headers.SetCookie = StringValues.Concat(headers.SetCookie, setCookieHeaderValue.ToString());
			// fim workaround

			// seta cookie com valor encodado AUTOMATICAMENTE
			response.Cookies.Append("myCookie", "foo=Val1&bar=Val2", new CookieOptions() { Expires = DateTime.UtcNow.AddDays(180) });
			response.Cookies.Append("ABC", "123");
			// seta cookie com valor encodado AUTOMATICAMENTE
		}

		/*
		public override void OnActionExecuted(ActionExecutedContext context)
		{
			context.HttpContext.Response.Headers
				.Add("filter-executed", "True");

			bool hasAttribute = context.ActionDescriptor.EndpointMetadata
				.Any(obj => obj is SomethingSomethingAttribute);

			context.HttpContext.Response.Headers
				.Add("filter-operation", hasAttribute.ToString());
		}
		*/
	}
}
