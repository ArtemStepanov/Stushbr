using MediatR;
using Stushbr.Data.DataAccess.Sql;

namespace Stushbr.Application.Abstractions;

public abstract class BaseRequestHandler<TRequest, TResult> : IRequestHandler<TRequest, TResult> where TRequest : IRequest<TResult>
{
    protected StushbrDbContext DbContext { get; }

    public BaseRequestHandler(StushbrDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public abstract Task<TResult> Handle(TRequest request, CancellationToken cancellationToken);
}