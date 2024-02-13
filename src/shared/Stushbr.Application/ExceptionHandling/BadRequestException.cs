using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Net;

namespace Stushbr.Application.ExceptionHandling
{
    public class BadRequestException(string? message = "Bad Request") : HttpException(message, HttpStatusCode.BadRequest)
    {
        public BadRequestException(ModelStateDictionary modelState) : this("Неверное состояние запроса")
        {
            Response.Errors.AddRange(modelState.SelectMany(pair => pair.Value.Errors, (pair, error) => new ErrorsResponse.Error
            {
                Property = pair.Key,
                Message = !string.IsNullOrEmpty(error.ErrorMessage)
                    ? error.ErrorMessage
                    : "Неверное значение свойства"
            }));
        }

        public BadRequestException(IEnumerable<ErrorsResponse.Error> errors) : this((string?)null)
        {
            Response.Errors.AddRange(errors);
        }

        public BadRequestException(ErrorsResponse.Error error) : this((string?)null)
        {
            Response.Errors.Add(error);
        }
    }
}
