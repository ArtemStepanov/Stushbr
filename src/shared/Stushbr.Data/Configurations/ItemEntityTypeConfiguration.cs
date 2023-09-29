using Microsoft.EntityFrameworkCore;
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
        builder.HasKey(c => c.Id);

        // Property Configurations
        builder.Property(c => c.DisplayName).IsRequired();

        builder.Property(c => c.Description).IsRequired();

        builder.Property(c => c.Price).IsRequired();

        builder.Property(c => c.Data)
            .HasConversion(
                v => v == null
                    ? null
                    : JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                v => string.IsNullOrEmpty(v)
                    ? null
                    : JsonSerializer.Deserialize<JsonNode>(v, (JsonSerializerOptions?)null));

        builder.Property(c => c.AvailableSince).IsRequired().HasDefaultValue(DateTime.UtcNow);

        builder.Property(i => i.ItemIdentifier).IsRequired();

        // Ignore the TelegramItemData property
        builder.HasOne(c => c.TelegramItem)
            .WithOne(x => x.Item).OnDelete(DeleteBehavior.Cascade);
    }
}