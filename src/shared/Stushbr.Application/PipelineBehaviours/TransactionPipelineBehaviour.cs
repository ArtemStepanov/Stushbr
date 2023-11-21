using MediatR;
using Microsoft.EntityFrameworkCore;
using Stushbr.Data.DataAccess.Sql;
using System.Data;

namespace Stushbr.Application.PipelineBehaviours;

public sealed class TransactionPipelineBehaviour<TRequest, TResult>(
    StushbrDbContext dbContext
) : IPipelineBehavior<TRequest, TResult>
    where TRequest : IRequest<TResult>
{
    public async Task<TResult> Handle(TRequest request, RequestHandlerDelegate<TResult> next, CancellationToken cancellationToken) =>
        await dbContext.Database.CreateExecutionStrategy().ExecuteAsync(async xct =>
        {
            var transaction = await dbContext.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted, xct);
            var result = await next();
            await dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(xct);
            return result;
        }, cancellationToken);
}