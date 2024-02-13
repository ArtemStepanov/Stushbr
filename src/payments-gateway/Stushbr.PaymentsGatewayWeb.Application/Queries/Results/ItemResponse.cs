using Stushbr.Core.Enums;

namespace Stushbr.PaymentsGatewayWeb.Application.Queries.Results;

public class ItemResponse
{
    public required string Id { get; set; }

    public required string DisplayName { get; set; }

    public required string Description { get; set; }

    public required string ImageUrl { get; set; }

    public double Price { get; set; }

    public ItemType Type { get; set; }

    public bool IsEnabled { get; set; }

    public DateTime AvailableSince { get; set; }

    public DateTime? AvailableBefore { get; set; }
}
