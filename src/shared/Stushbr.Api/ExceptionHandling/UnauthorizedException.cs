using System.Net;

namespace Stushbr.Api.ExceptionHandling
{
    public class UnauthorizedException : HttpException
    {
        public UnauthorizedException(string message = "Unauthorized") : base(message, HttpStatusCode.Unauthorized)
        {
        }
    }
}