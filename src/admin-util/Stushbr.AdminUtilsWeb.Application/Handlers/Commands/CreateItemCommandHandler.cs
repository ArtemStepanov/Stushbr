using Stushbr.AdminUtilsWeb.Domain.Items.Commands;
using Stushbr.AdminUtilsWeb.Domain.Items.Contracts;
using Stushbr.Application.Abstractions;
using Stushbr.Core.Enums;
using Stushbr.Data.DataAccess.Sql;
using Stushbr.Domain.Models.Items;

namespace Stushbr.AdminUtilsWeb.Application.Handlers.Commands;

public sealed class CreateItemCommandHandler(StushbrDbContext dbContext) : BaseRequestHandler<CreateItemCommand, ItemViewModel>(dbContext)
{
    public override async Task<ItemViewModel> Handle(CreateItemCommand request, CancellationToken cancellationToken)
    {
        var item = new Item
        {
            DisplayName = request.DisplayName,
            Description = request.Description,
            Price = request.Price,
            ItemIdentifier = request.ItemIdentifier,
            Type = Enum.Parse<ItemType>(request.Type),
            IsEnabled = true,
            AvailableSince = request.AvailableSince,
            AvailableBefore = request.AvailableBefore
        };

        await dbContext.Items.AddAsync(item, cancellationToken);

        return ItemViewModel.Create.Compile()(item);
    }
}