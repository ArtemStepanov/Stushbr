using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stushbr.Domain.Models;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Stushbr.Data.Configurations;

public sealed class ClientItemTypeConfiguration : IEntityTypeConfiguration<ClientItem>
{
    public void Configure(EntityTypeBuilder<ClientItem> builder)
    {
        // Primary Key Configuration
        builder.HasKey(c => c.Id);

        // Property Configurations
        builder.Property(c => c.ClientId)
            .IsRequired();

        builder.Property(c => c.ItemId)
            .IsRequired();

        builder.Property(c => c.PaymentSystemBillId);

        builder.Property(c => c.PaymentSystemBillDueDate);

        builder.Property(c => c.IsPaid);

        builder.Property(c => c.IsProcessed);

        builder.Property(c => c.PaymentDate);

        builder.Property(c => c.ProcessDate);

        builder.Property(c => c.Data)
            .HasColumnType("jsonb")
            .HasConversion(
                v => v == null
                    ? null
                    : JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                v => v == null
                    ? null
                    : JsonSerializer.Deserialize<JsonNode>(v, (JsonSerializerOptions?)null)
            );

        // Ignore the TelegramData property
        builder.Ignore(c => c.TelegramData);

        // Relationship Configurations
        builder.HasOne(c => c.AssociatedClient)
            .WithMany()
            .HasForeignKey(c => c.ClientId);

        builder.HasOne(c => c.AssociatedItem)
            .WithMany()
            .HasForeignKey(c => c.ItemId);
    }
}