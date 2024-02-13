using MediatR;

namespace Stushbr.Core.Mediatr.Abstractions;

public interface IQuery : IRequest;

public interface IQuery<out TResult> : IRequest<TResult>;