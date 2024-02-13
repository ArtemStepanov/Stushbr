using Stushbr.AdminUtilsWeb.Domain.Items.Commands;
using Stushbr.Application.Abstractions;
using Stushbr.Application.ExceptionHandling;
using Stushbr.Data.DataAccess.Sql;

namespace Stushbr.AdminUtilsWeb.Application.Handlers.Commands;

public sealed class DeleteItemCommandHandler(StushbrDbContext dbContext) : BaseRequestHandler<DeleteItemCommand, bool>(dbContext)
{
    public override async Task<bool> Handle(DeleteItemCommand request, CancellationToken cancellationToken)
    {
        var item = await DbContext.Items.FindAsync([request.ItemId], cancellationToken) ?? throw new NotFoundException();
        DbContext.Remove(item);

        return true;
    }
}