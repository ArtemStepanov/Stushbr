using Microsoft.EntityFrameworkCore;
using Stushbr.AdminUtilsWeb.Domain.Items.Contracts;
using Stushbr.AdminUtilsWeb.Domain.Items.Queries;
using Stushbr.Application.Abstractions;
using Stushbr.Data.DataAccess.Sql;

namespace Stushbr.AdminUtilsWeb.Application.Handlers.Queries;

public sealed class GetItemsQueryHandler(StushbrDbContext dbContext) : BaseRequestHandler<GetItemsQuery, IReadOnlyList<ItemViewModel>>(dbContext)
{
    public override async Task<IReadOnlyList<ItemViewModel>> Handle(GetItemsQuery request, CancellationToken cancellationToken)
    {
        return await dbContext.Items.Select(ItemViewModel.Create).ToListAsync(cancellationToken);
    }
}