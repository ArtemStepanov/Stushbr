using System.Net;

namespace Stushbr.Application.ExceptionHandling
{
    public class NotFoundException(string message = "Not Found") : HttpException(message, HttpStatusCode.NotFound);
}