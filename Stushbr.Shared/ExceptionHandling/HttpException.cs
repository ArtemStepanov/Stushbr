using System.Net;

namespace Stushbr.Shared.ExceptionHandling
{
    public class HttpException : Exception
    {
        public HttpStatusCode HttpCode { get; }

        public ErrorsResponse Response { get; } = new();

        protected void TryAddErrorMessage(string? message)
        {
            if (message != null)
            {
                Response.Errors.Add(new ErrorsResponse.Error(message));
            }
        }

        public HttpException(string? message = "Internal Server Error", HttpStatusCode httpCode = HttpStatusCode.InternalServerError) : base(message)
        {
            HttpCode = httpCode;
            TryAddErrorMessage(message);
        }

        public HttpException(string? message, Exception innerException, HttpStatusCode httpCode = HttpStatusCode.InternalServerError) : this(message, httpCode)
        {
            TryAddErrorMessage(innerException.Message);
        }

        public HttpException(Exception exception) : this(exception.Message)
        {
        }

        public HttpException(IEnumerable<ErrorsResponse.Error> errors) : this((string?)null)
        {
            Response.Errors.AddRange(errors);
        }

        public HttpException(HttpStatusCode httpCode, IEnumerable<ErrorsResponse.Error> errors) : this((string?)null)
        {
            HttpCode = httpCode;
            Response.Errors.AddRange(errors);
        }

        public override string Message => Response.Errors.Any()
            ? "Errors are:\n" + string.Join("\n", Response.Errors)
            : base.Message;
    }
}