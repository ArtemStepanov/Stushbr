using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stushbr.Domain.Models;
using Stushbr.Domain.Models.Clients;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Stushbr.Data.Configurations;

public sealed class TelegramClientItemTypeConfiguration : IEntityTypeConfiguration<TelegramClientItem>
{
    public void Configure(EntityTypeBuilder<TelegramClientItem> builder)
    {
        // Primary Key Configuration
        builder.HasKey(c => c.Id);
    }
}