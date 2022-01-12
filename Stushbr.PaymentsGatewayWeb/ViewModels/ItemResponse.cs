using Stushbr.PaymentsGatewayWeb.Models;

namespace Stushbr.PaymentsGatewayWeb.ViewModels;

public record ItemResponse(
    string Id,
    string DisplayName,
    string Description,
    double Price,
    ItemType Type,
    bool IsAvailable,
    DateTime AvailableSince,
    DateTime AvailableBefore
);