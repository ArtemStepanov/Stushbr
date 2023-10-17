using System.Net;

namespace Stushbr.Application.ExceptionHandling
{
    public class NotFoundException : HttpException
    {
        public NotFoundException(string message = "Not Found") : base(message, HttpStatusCode.NotFound)
        {
        }
    }
}