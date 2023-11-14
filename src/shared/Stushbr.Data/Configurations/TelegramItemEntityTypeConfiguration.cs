using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stushbr.Domain.Models.Items;

namespace Stushbr.Data.Configurations;

public sealed class TelegramItemEntityTypeConfiguration : IEntityTypeConfiguration<TelegramItem>
{
    public void Configure(EntityTypeBuilder<TelegramItem> builder)
    {
        builder.HasMany(x => x.Channels)
            .WithOne(x => x.TelegramItem).OnDelete(DeleteBehavior.Cascade);
    }
}