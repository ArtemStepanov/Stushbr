using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stushbr.Domain.Models;

namespace Stushbr.Data.Configurations;

public sealed class ClientEntityTypeConfiguration : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.FirstName)
            .IsRequired()
            .HasColumnType("nvarchar(100)");

        builder.Property(c => c.SecondName)
            .IsRequired()
            .HasColumnType("nvarchar(100)");

        builder.Property(c => c.Email)
            .IsRequired()
            .HasColumnType("nvarchar(100)");

        builder.Property(c => c.PhoneNumber)
            .IsRequired()
            .HasColumnType("nvarchar(20)");

        builder.Ignore(c => c.FullName);
    }
}