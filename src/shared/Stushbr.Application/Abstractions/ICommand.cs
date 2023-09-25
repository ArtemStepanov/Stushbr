using MediatR;

namespace Stushbr.Application.Abstractions;

public interface ICommand : IRequest
{
}

public interface ICommand<out TResult> : IRequest<TResult>
{
}
