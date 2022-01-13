using LinqToDB.Mapping;
using Stushbr.Shared.DataAccess;

namespace Stushbr.PaymentsGatewayWeb.Models;

[Table("Bills")]
public class Bill : IIdentifier
{
    [PrimaryKey]
    public string Id { get; set; } = Guid.NewGuid().ToString("N");

    [Column, NotNull]
    public string ClientId { get; set; }

    [Association(ThisKey = nameof(ClientId), OtherKey = nameof(Client.Id), Relationship = Relationship.ManyToOne)]
    public Client? AssociatedClient { get; set; }

    [Column, NotNull]
    public string ItemId { get; set; }

    [Association(ThisKey = nameof(ItemId), OtherKey = nameof(Item.Id), Relationship = Relationship.ManyToOne)]
    public Item? AssociatedItem { get; set; }

    [Column]
    public string? PaymentSystemBillId { get; set; }
}
