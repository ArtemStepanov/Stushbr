using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stushbr.Domain.Models.Clients;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Stushbr.Data.Configurations;

public sealed class ClientItemTypeConfiguration : IEntityTypeConfiguration<ClientItem>
{
    public void Configure(EntityTypeBuilder<ClientItem> builder)
    {
        // Primary Key Configuration
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Data)
            .HasConversion(
                v => v == null
                    ? null
                    : JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                v => v == null
                    ? null
                    : JsonSerializer.Deserialize<JsonNode>(v, (JsonSerializerOptions?)null));

        // Relationship Configurations
        builder.HasOne(c => c.Client)
            .WithMany(x => x.ClientItems).OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(c => c.TelegramData)
            .WithOne(x => x.ClientItem).OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.Item)
            .WithMany(i => i.ClientItems).OnDelete(DeleteBehavior.Restrict);
    }
}