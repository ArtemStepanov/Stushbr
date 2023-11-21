using MediatR;
using Stushbr.Data.DataAccess.Sql;

namespace Stushbr.Application.Abstractions;

public abstract class BaseRequestHandler<TRequest, TResult>(StushbrDbContext dbContext) : IRequestHandler<TRequest, TResult>
    where TRequest : IRequest<TResult>
{
    protected StushbrDbContext DbContext { get; } = dbContext;

    public abstract Task<TResult> Handle(TRequest request, CancellationToken cancellationToken);
}