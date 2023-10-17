using System.Net;

namespace Stushbr.Application.ExceptionHandling
{
    public class UnauthorizedException : HttpException
    {
        public UnauthorizedException(string message = "Unauthorized") : base(message, HttpStatusCode.Unauthorized)
        {
        }
    }
}