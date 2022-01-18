using LinqToDB;
using LinqToDB.Mapping;
using Stushbr.Shared.DataAccess;
using Stushbr.Shared.Extensions;
using System.Text.Json.Nodes;

namespace Stushbr.Shared.Models;

[Table("client_items")]
public class ClientItem : IIdentifier
{
    [Column("id"), PrimaryKey]
    public string Id { get; set; } = Guid.NewGuid().ToString("N");

    [Column("client_id")]
    public string ClientId { get; set; }

    [Association(ThisKey = nameof(ClientId), OtherKey = nameof(Client.Id))]
    public Client AssociatedClient { get; set; }

    [Column("item_id")]
    public string ItemId { get; set; }

    [Association(ThisKey = nameof(ItemId), OtherKey = nameof(Item.Id))]
    public Item AssociatedItem { get; set; }

    [Column("payment_system_bill_id")]
    public string? PaymentSystemBillId { get; set; }

    [Column("is_paid")]
    public bool IsPaid { get; set; }

    [Column("is_processed")]
    public bool IsProcessed { get; set; }

    [Column("payment_date")]
    public DateTime? PaymentDate { get; set; }

    [Column("process_date")]
    public DateTime? ProcessDate { get; set; }

    [Column("data", DataType = DataType.Json)]
    public JsonNode Data { get; set; }

    public TelegramClientItemData? GetTelegramData()
    {
        return Data.ToObject<TelegramClientItemData>();
    }
}
