using System.Net;

namespace Stushbr.Shared.ExceptionHandling
{
    public class UnauthorizedException : HttpException
    {
        public UnauthorizedException(string message = "Unauthorized") : base(message, HttpStatusCode.Unauthorized)
        {
        }
    }
}