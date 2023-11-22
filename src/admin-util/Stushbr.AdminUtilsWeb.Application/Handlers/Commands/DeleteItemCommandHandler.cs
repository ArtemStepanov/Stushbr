using Stushbr.AdminUtilsWeb.Domain.Items.Commands;
using Stushbr.Application.Abstractions;
using Stushbr.Application.ExceptionHandling;
using Stushbr.Data.DataAccess.Sql;

namespace Stushbr.AdminUtilsWeb.Application.Handlers.Commands;

public sealed class DeleteItemCommandHandler(StushbrDbContext dbContext) : BaseRequestHandler<DeleteItemCommand, bool>(dbContext)
{
    public override async Task<bool> Handle(DeleteItemCommand request, CancellationToken cancellationToken)
    {
        var item = await dbContext.Items.FindAsync(new object?[] { request.ItemId }, cancellationToken);
        if (item is null)
        {
            throw new NotFoundException();
        }

        dbContext.Remove(item);

        return true;
    }
}