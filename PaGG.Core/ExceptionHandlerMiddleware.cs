using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PaGG.Core
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHostEnvironment _hostEnvironment;
        private readonly string ContentTypeHeader = "application/json";

        public ExceptionHandlerMiddleware(RequestDelegate next, IHostEnvironment hostEnvironment)
        {
            _next = next;
            _hostEnvironment = hostEnvironment;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var response = context.Response;
                response.ContentType = ContentTypeHeader;

                (HttpStatusCode statusCode, string message, string debug) = DefaultHandler(ex, _hostEnvironment.IsDevelopment());

                response.StatusCode = (int)statusCode;
                var result = JsonSerializer.Serialize(new { message, debug });
                await response.WriteAsync(result);
            }
        }

        private static Func<Exception, bool, (HttpStatusCode, string, string)> DefaultHandler = (exception, isDevelopment) =>
        {
            var statusCode = exception is PaGGCustomException ex
                ? ex.StatusCode
                : HttpStatusCode.InternalServerError;
            
            (string message, string stackTrace) = ExtractMessage(exception, isDevelopment);

            return (statusCode, message, stackTrace);
        };

        private static Func<Exception, bool, (string, string)> ExtractMessage = (exception, isDevelopment) =>
        {
            var message = new StringBuilder()
                .AppendLine(exception.Message);
                //.AppendLine();

            var debug = new StringBuilder();

            Exception innerException = exception.InnerException;

            while (innerException != null)
            {
                message.AppendLine(innerException.Message);
                //message.AppendLine();
                innerException = innerException.InnerException;
            }

            if (isDevelopment)
                debug.AppendLine(exception.StackTrace);

            return (message.ToString(), debug.ToString());
        };
    }
}
