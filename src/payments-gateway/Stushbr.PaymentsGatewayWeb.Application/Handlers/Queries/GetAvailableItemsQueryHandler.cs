using Microsoft.EntityFrameworkCore;
using Stushbr.Application.Abstractions;
using Stushbr.Data.DataAccess.Sql;
using Stushbr.Domain.Models.Items;
using Stushbr.PaymentsGatewayWeb.Application.Queries;

namespace Stushbr.PaymentsGatewayWeb.Application.Handlers.Queries;

public sealed class GetAvailableItemsQueryHandler(StushbrDbContext dbContext) : BaseRequestHandler<GetAvailableItemsQuery, IReadOnlyCollection<Item>>(dbContext)
{
    public override async Task<IReadOnlyCollection<Item>> Handle(GetAvailableItemsQuery request, CancellationToken cancellationToken)
    {
        var items = await DbContext.Items
            .Where(i => i.IsEnabled && DateTime.Now > i.AvailableSince && (i.AvailableBefore == null || DateTime.Now < i.AvailableBefore))
            .ToListAsync(cancellationToken);

        return items;
    }
}