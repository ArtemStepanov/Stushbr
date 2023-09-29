using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stushbr.Domain.Models;
using Stushbr.Domain.Models.Clients;

namespace Stushbr.Data.Configurations;

public sealed class ClientEntityTypeConfiguration : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.FirstName).IsRequired().HasMaxLength(100);

        builder.Property(c => c.SecondName).IsRequired().HasMaxLength(100);

        builder.Property(c => c.Email).IsRequired().HasMaxLength(100);

        builder.Property(c => c.PhoneNumber).IsRequired().HasMaxLength(20);

        builder.Ignore(c => c.FullName);

        builder.HasMany(c => c.ClientItems)
            .WithOne(x => x.Client)
            .OnDelete(DeleteBehavior.Cascade);
    }
}