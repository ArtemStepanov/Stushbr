using MediatR;
using Stushbr.Com.Shared.Abstractions;

namespace Stushbr.Com.Application.Abstractions;

public abstract class BaseRequestHandler<TRequest, TResult> : IRequestHandler<TRequest, TResult> where TRequest : IRequest<TResult>
{
    public abstract Task<TResult> Handle(TRequest request, CancellationToken cancellationToken);
}
