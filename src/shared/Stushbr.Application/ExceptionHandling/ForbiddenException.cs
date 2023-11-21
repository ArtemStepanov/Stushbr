using System.Net;

namespace Stushbr.Application.ExceptionHandling
{
    public class ForbiddenException(string message = "Forbidden") : HttpException(message, HttpStatusCode.Forbidden);
}