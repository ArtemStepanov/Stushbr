using Stushbr.Core.Enums;
using Stushbr.Domain.Abstractions;
using Stushbr.Domain.Models.Clients;
using System.Text.Json.Nodes;

namespace Stushbr.Domain.Models.Items;

public class Item : IIdentifier
{
    public int Id { get; set; }

    public string DisplayName { get; set; } = default!;

    public string Description { get; set; } = default!;

    public string? ImageUrl { get; set; }

    public double Price { get; set; }

    public string ItemIdentifier { get; set; } = default!;

    public ItemType Type { get; set; }

    public JsonNode? Data { get; set; }

    public bool IsEnabled { get; set; } = true;

    public DateTime AvailableSince { get; set; } = DateTime.Now;

    public DateTime? AvailableBefore { get; set; }

    public virtual ICollection<ClientItem> ClientItems { get; set; } = new List<ClientItem>();

    public TelegramItem? TelegramItem { get; set; }

    public bool IsAvailable => IsEnabled && DateTime.Now > AvailableSince && (AvailableBefore == null || DateTime.Now < AvailableBefore);
}