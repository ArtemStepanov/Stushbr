using MediatR;

namespace Stushbr.Application.Abstractions;

public abstract class BaseRequestHandler<TRequest, TResult> : IRequestHandler<TRequest, TResult> where TRequest : IRequest<TResult>
{
    public abstract Task<TResult> Handle(TRequest request, CancellationToken cancellationToken);
}
