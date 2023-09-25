﻿using Stushbr.Core.Extensions;
using Stushbr.Domain.Abstractions;
using System.Text.Json.Nodes;

namespace Stushbr.Domain.Models;

public class Item : IIdentifier
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N");

    public string DisplayName { get; set; }

    public string Description { get; set; }

    public string? ImageUrl { get; set; }

    public double Price { get; set; }

    public ItemType Type { get; set; }

    public JsonNode? Data { get; set; }

    public bool IsEnabled { get; set; } = true;

    public DateTime AvailableSince { get; set; } = DateTime.Now;

    public DateTime? AvailableBefore { get; set; }

    public virtual ICollection<ClientItem> ClientItems { get; set; } = new List<ClientItem>();

    // Unmapped property
    public TelegramItemData? TelegramItemData => Data?.ToObject<TelegramItemData>();
}
