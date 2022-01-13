namespace Stushbr.PaymentsGatewayWeb.Models;

/// <summary>
/// Item, that is owned by <paramref name="ClientId"/>
/// </summary>
/// <param name="ItemId">Item id</param>
/// <param name="OwnedSince">Date, when the item was bought</param>
/// <param name="OwnedBefore">Date, when access to the item should be revoked</param>
/// <param name="IsReceiptSent">Is receipt sent for that item?</param>
public record OwnedItem(
    string ItemId,
    string BillId,
    DateTime OwnedSince,
    DateTime OwnedBefore,
    bool IsReceiptSent
);
