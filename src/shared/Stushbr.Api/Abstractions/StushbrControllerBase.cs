using MediatR;
using Microsoft.AspNetCore.Mvc;
using Stushbr.Application.ExceptionHandling;

namespace Stushbr.Api.Abstractions;

public abstract class StushbrControllerBase(ISender sender) : Controller
{
    protected async Task<IActionResult> CallHandlerAsync<TResult>(IRequest<TResult> request, Func<TResult, IActionResult> successAction)
    {
        if (!ModelState.IsValid)
        {
            throw new BadRequestException(ModelState);
        }

        var result = await sender.Send(request);

        return successAction(result);
    }
}