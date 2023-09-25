using MediatR;
using Microsoft.EntityFrameworkCore;
using Stushbr.Data.DataAccess.Postgres;
using System.Data;

namespace Stushbr.Application.PipelineBehaviours;

public sealed class TransactionPipelineBehaviour<TRequest, TResult> : IPipelineBehavior<TRequest, TResult> where TRequest : IRequest<TResult>
{
    private readonly StushbrDbContext _dbContext;

    public TransactionPipelineBehaviour(StushbrDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<TResult> Handle(TRequest request, RequestHandlerDelegate<TResult> next, CancellationToken cancellationToken) =>
        await _dbContext.Database.CreateExecutionStrategy().ExecuteAsync(async xct =>
        {
            var transaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted, xct);
            var result = await next();
            await transaction.CommitAsync(xct);
            return result;
        }, cancellationToken);
}