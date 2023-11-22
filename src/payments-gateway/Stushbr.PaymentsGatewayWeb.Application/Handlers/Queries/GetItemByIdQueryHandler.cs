using Stushbr.Application.Abstractions;
using Stushbr.Application.ExceptionHandling;
using Stushbr.Application.Extensions;
using Stushbr.Data.DataAccess.Sql;
using Stushbr.Domain.Models.Items;
using Stushbr.PaymentsGatewayWeb.Application.Queries;

namespace Stushbr.PaymentsGatewayWeb.Application.Handlers.Queries;

public sealed class GetItemByIdQueryHandler(StushbrDbContext dbContext) : BaseRequestHandler<GetItemByIdQuery, Item>(dbContext)
{
    public override async Task<Item> Handle(GetItemByIdQuery request, CancellationToken cancellationToken)
    {
        var item = await DbContext.Items.FindOrThrowAsync(request.Id, "Продукт не найден", cancellationToken);
        if (!item.IsAvailable)
        {
            throw new BadRequestException("Продукт неактивен");
        }

        return item;
    }
}