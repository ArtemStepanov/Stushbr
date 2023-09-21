using MediatR;

namespace Stushbr.Shared.Abstractions;

public interface ICommand : IRequest
{
}

public interface ICommand<out TResult> : IRequest<TResult>
{
}
