using System.Net;

namespace Stushbr.Api.ExceptionHandling
{
    /// <summary>
    /// Exception that indicates that resource can't be created because of conflict.
    /// For example if entity with the same id or name exists.
    /// </summary>
    public class ConflictException : HttpException
    {
        public ConflictException(string message = "Resource with the same id already exists.") : base(message, HttpStatusCode.Conflict)
        {
        }
    }
}