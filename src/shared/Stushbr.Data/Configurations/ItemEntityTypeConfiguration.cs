using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stushbr.Domain.Models;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Stushbr.Data.Configurations;

public sealed class ItemEntityTypeConfiguration : IEntityTypeConfiguration<Item>
{
    public void Configure(EntityTypeBuilder<Item> builder)
    {
        // Primary Key Configuration
        builder.HasKey(c => c.Id);

        // Property Configurations
        builder.Property(c => c.DisplayName)
            .IsRequired();

        builder.Property(c => c.Description)
            .IsRequired();

        builder.Property(c => c.ImageUrl);

        builder.Property(c => c.Price);

        builder.Property(c => c.Type);

        builder.Property(c => c.Data)
            .HasColumnType("jsonb")
            .HasConversion(
                v => v == null
                    ? null
                    : JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                v => string.IsNullOrEmpty(v)
                    ? null
                    : JsonSerializer.Deserialize<JsonNode>(v, (JsonSerializerOptions?)null));

        builder.Property(c => c.IsEnabled);

        builder.Property(c => c.AvailableSince);

        builder.Property(c => c.AvailableBefore);

        // Ignore the TelegramItemData property
        builder.Ignore(c => c.TelegramItemData);

        // Relationship Configurations
        builder.HasMany(c => c.ClientItems)
            .WithOne(i => i.AssociatedItem)
            .HasForeignKey(c => c.ItemId);
    }
}