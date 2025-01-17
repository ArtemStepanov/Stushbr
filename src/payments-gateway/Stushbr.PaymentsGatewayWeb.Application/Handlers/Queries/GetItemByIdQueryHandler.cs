using Microsoft.EntityFrameworkCore;
using Stushbr.Application.Abstractions;
using Stushbr.Application.ExceptionHandling;
using Stushbr.Data.DataAccess.Sql;
using Stushbr.Domain.Models.Items;
using Stushbr.PaymentsGatewayWeb.Application.Queries;

namespace Stushbr.PaymentsGatewayWeb.Application.Handlers.Queries;

public sealed class GetItemByIdQueryHandler(StushbrDbContext dbContext) : BaseRequestHandler<GetItemByIdQuery, Item>(dbContext)
{
    public override async Task<Item> Handle(GetItemByIdQuery request, CancellationToken cancellationToken)
    {
        var item = await DbContext.Items.FirstOrDefaultAsync(x => x.Id == request.Id && x.IsAvailable, cancellationToken);
        if (item is null)
        {
            throw new NotFoundException("Продукт не найден");
        }

        return item;
    }
}