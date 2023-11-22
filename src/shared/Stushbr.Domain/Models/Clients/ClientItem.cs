using Stushbr.Domain.Abstractions;
using Stushbr.Domain.Models.Items;
using System.Text.Json.Nodes;

namespace Stushbr.Domain.Models.Clients;

public class ClientItem : IIdentifier
{
    public int Id { get; set; }

    public int ClientId { get; set; } = default!;

    public Client? Client { get; set; }

    public int ItemId { get; set; } = default!;

    public Item? Item { get; set; }

    public string? PaymentSystemBillId { get; set; }

    public DateTime? PaymentSystemBillDueDate { get; set; }

    public bool IsPaid { get; set; }

    public bool IsProcessed { get; private set; }

    public DateTime? PaymentDate { get; set; }

    public DateTime? ProcessDate { get; private set; }

    public JsonNode? Data { get; set; }

    public virtual ICollection<TelegramClientItem> TelegramData { get; set; } = new List<TelegramClientItem>();

    public void SetProcessed(bool isProcessed)
    {
        IsProcessed = isProcessed;
        ProcessDate = isProcessed ? DateTime.Now : null;
    }
}
