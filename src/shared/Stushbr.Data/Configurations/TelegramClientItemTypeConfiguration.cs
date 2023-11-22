using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stushbr.Domain.Models.Clients;

namespace Stushbr.Data.Configurations;

public sealed class TelegramClientItemTypeConfiguration : IEntityTypeConfiguration<TelegramClientItem>
{
    public void Configure(EntityTypeBuilder<TelegramClientItem> builder)
    {
    }
}