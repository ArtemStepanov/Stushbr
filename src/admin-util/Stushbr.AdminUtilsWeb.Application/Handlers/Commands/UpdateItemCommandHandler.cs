using Stushbr.AdminUtilsWeb.Domain.Items.Commands;
using Stushbr.AdminUtilsWeb.Domain.Items.Contracts;
using Stushbr.Application.Abstractions;
using Stushbr.Application.ExceptionHandling;
using Stushbr.Core.Enums;
using Stushbr.Data.DataAccess.Sql;

namespace Stushbr.AdminUtilsWeb.Application.Handlers.Commands;

public sealed class UpdateItemCommandHandler(StushbrDbContext dbContext) : BaseRequestHandler<UpdateItemCommand, ItemViewModel>(dbContext)
{
    public override async Task<ItemViewModel> Handle(UpdateItemCommand request, CancellationToken cancellationToken)
    {
        var item = await dbContext.Items.FindAsync(new object?[] { request.Id }, cancellationToken: cancellationToken);
        if (item is null)
        {
            throw new NotFoundException();
        }

        item.DisplayName = request.DisplayName;
        item.Description = request.Description;
        item.Price = request.Price;
        item.ItemIdentifier = request.ItemIdentifier;
        item.Type = Enum.Parse<ItemType>(request.Type);
        item.IsEnabled = request.IsEnabled;
        item.AvailableSince = request.AvailableSince;
        item.AvailableBefore = request.AvailableBefore;

        return ItemViewModel.Create.Compile()(item);
    }
}