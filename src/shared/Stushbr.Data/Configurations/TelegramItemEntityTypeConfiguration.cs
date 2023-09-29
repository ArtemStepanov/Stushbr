using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stushbr.Domain.Models;
using Stushbr.Domain.Models.Items;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Stushbr.Data.Configurations;

public sealed class TelegramItemEntityTypeConfiguration : IEntityTypeConfiguration<TelegramItem>
{
    public void Configure(EntityTypeBuilder<TelegramItem> builder)
    {
        // Primary Key Configuration
        builder.HasKey(c => c.Id);

        // Property Configurations
        builder.Property(x => x.ItemId).IsRequired();
    }
}