using MediatR;

namespace Stushbr.Core.Mediatr.Abstractions;

public interface ICommand : IRequest;

public interface ICommand<out TResult> : IRequest<TResult>;