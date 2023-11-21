using System.Net;

namespace Stushbr.Application.ExceptionHandling
{
    /// <summary>
    /// Exception that indicates that resource can't be created because of conflict.
    /// For example if entity with the same id or name exists.
    /// </summary>
    public class ConflictException(string message = "Resource with the same id already exists.")
        : HttpException(message, HttpStatusCode.Conflict);
}