using System.Net;

namespace Stushbr.Application.ExceptionHandling
{
    public class UnauthorizedException(string message = "Unauthorized") : HttpException(message, HttpStatusCode.Unauthorized);
}