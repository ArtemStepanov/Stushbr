using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stushbr.Domain.Models.Items;

namespace Stushbr.Data.Configurations;

public sealed class TelegramItemChannelEntityTypeConfiguration : IEntityTypeConfiguration<TelegramItemChannel>
{
    public void Configure(EntityTypeBuilder<TelegramItemChannel> builder)
    {
        builder.HasKey(x => new { x.TelegramItemId, x.ChannelId });
    }
}