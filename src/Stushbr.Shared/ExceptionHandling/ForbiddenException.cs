using System.Net;

namespace Stushbr.Shared.ExceptionHandling
{
    public class ForbiddenException : HttpException
    {
        public ForbiddenException(string message = "Forbidden") : base(message, HttpStatusCode.Forbidden)
        {
        }
    }
}