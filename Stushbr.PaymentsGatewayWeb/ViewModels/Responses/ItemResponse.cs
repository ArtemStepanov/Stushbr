using Stushbr.Shared.Models;

namespace Stushbr.PaymentsGatewayWeb.ViewModels.Responses;

public class ItemResponse
{
    public string Id { get; set; }

    public string DisplayName { get; set; }

    public string Description { get; set; }

    public string ImageUrl { get; set; }

    public double Price { get; set; }

    public ItemType Type { get; set; }

    public bool IsEnabled { get; set; }

    public DateTime AvailableSince { get; set; }

    public DateTime? AvailableBefore { get; set; }
}
