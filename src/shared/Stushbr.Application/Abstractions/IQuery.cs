using MediatR;

namespace Stushbr.Application.Abstractions;

public interface IQuery : IRequest
{
}

public interface IQuery<out TResult> : IRequest<TResult>
{
}