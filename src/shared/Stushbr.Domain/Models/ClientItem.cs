using Stushbr.Core.Extensions;
using Stushbr.Domain.Abstractions;
using System.Runtime.Serialization;
using System.Text.Json.Nodes;

namespace Stushbr.Domain.Models;

public class ClientItem : IIdentifier
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N");

    public string ClientId { get; set; } = default!;

    public Client? Client { get; set; }

    public string ItemId { get; set; } = default!;

    public Item? Item { get; set; }

    public string? PaymentSystemBillId { get; set; }

    public DateTime? PaymentSystemBillDueDate { get; set; }

    public bool IsPaid { get; set; }

    public bool IsProcessed { get; set; }

    public DateTime? PaymentDate { get; set; }

    public DateTime? ProcessDate { get; set; }

    public JsonNode? Data { get; set; }

    [IgnoreDataMember]
    public TelegramClientItemDataWrapper? TelegramData
    {
        get => Data?.ToObject<TelegramClientItemDataWrapper>();
        set => Data = value?.JsonNodeFromObject();
    }

    public void SetProcessed(bool isProcessed)
    {
        IsProcessed = isProcessed;
        ProcessDate = isProcessed ? DateTime.Now : null;
    }
}
