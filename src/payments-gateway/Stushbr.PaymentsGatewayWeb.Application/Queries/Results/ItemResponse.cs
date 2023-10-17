using Stushbr.Core.Enums;

namespace Stushbr.PaymentsGatewayWeb.Application.Queries.Results;

public class ItemResponse
{
    public string Id { get; set; } = default!;

    public string DisplayName { get; set; } = default!;

    public string Description { get; set; } = default!;

    public string ImageUrl { get; set; } = default!;

    public double Price { get; set; } = default!;

    public ItemType Type { get; set; } = default!;

    public bool IsEnabled { get; set; } = default!;

    public DateTime AvailableSince { get; set; } = default!;

    public DateTime? AvailableBefore { get; set; } = default!;
}
