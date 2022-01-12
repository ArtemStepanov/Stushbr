using Stushbr.Shared.DataAccess;

namespace Stushbr.PaymentsGatewayWeb.Models;

/// <summary>
/// Client entity
/// </summary>
/// <param name="Id"></param>
/// <param name="FirstName"></param>
/// <param name="SecondName"></param>
/// <param name="Email"></param>
/// <param name="PhoneNumber"></param>
/// <param name="OwnedItems"></param>
public record Client(
    string Id,
    string FirstName,
    string SecondName,
    string Email,
    string PhoneNumber,
    List<OwnedItem> OwnedItems
) : IIdentifier;
