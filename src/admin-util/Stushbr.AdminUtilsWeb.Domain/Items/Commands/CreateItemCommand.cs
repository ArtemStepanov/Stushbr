using Stushbr.AdminUtilsWeb.Domain.Items.Contracts;
using Stushbr.Core.Mediatr.Abstractions;

namespace Stushbr.AdminUtilsWeb.Domain.Items.Commands;

public sealed class CreateItemCommand : ICommand<ItemViewModel>
{
    public required string DisplayName { get; set; }

    public required string Description { get; set; }

    public double Price { get; set; }

    public required string ItemIdentifier { get; set; }

    public required string Type { get; set; }

    public DateTime AvailableSince { get; set; } = DateTime.Now;

    public DateTime AvailableBefore { get; set; } = DateTime.Now.AddDays(10);
}