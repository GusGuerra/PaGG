using System;
using System.Net;

namespace PaGG.Core.Exceptions
{
    public class PaGGCustomException : Exception
    {
        public HttpStatusCode StatusCode;

        public PaGGCustomException() : this("Something went wrong") { }
        public PaGGCustomException(string message) : this (HttpStatusCode.InternalServerError, message) { }
        public PaGGCustomException(HttpStatusCode statusCode, string message) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}