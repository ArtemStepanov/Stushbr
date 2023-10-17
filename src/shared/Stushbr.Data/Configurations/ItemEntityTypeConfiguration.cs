﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stushbr.Domain.Models;
using Stushbr.Domain.Models.Items;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Stushbr.Data.Configurations;

public sealed class ItemEntityTypeConfiguration : IEntityTypeConfiguration<Item>
{
    public void Configure(EntityTypeBuilder<Item> builder)
    {
        // Primary Key Configuration
        builder.HasKey(x => x.Id);

        // Property Configurations
        builder.Property(x => x.DisplayName).IsRequired();

        builder.Property(x => x.Description).IsRequired();

        builder.Property(x => x.Price).IsRequired();

        builder.Property(i => i.Data)
            .HasConversion(
                x => x == null
                    ? null
                    : JsonSerializer.Serialize(x, (JsonSerializerOptions?)null),
                x => string.IsNullOrEmpty(x)
                    ? null
                    : JsonSerializer.Deserialize<JsonNode>(x, (JsonSerializerOptions?)null));

        builder.Property(x => x.AvailableSince).IsRequired().HasDefaultValue(DateTime.UtcNow);

        builder.HasIndex(x => x.ItemIdentifier).IsUnique(false);

        builder.HasOne(x => x.TelegramItem)
            .WithOne(x => x.Item).OnDelete(DeleteBehavior.Cascade);

        builder.Ignore(x => x.IsAvailable);
    }
}