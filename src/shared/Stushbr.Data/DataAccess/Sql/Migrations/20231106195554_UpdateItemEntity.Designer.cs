﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Stushbr.Data.DataAccess.Sql;

#nullable disable

namespace Stushbr.Data.DataAccess.Sql.Migrations
{
    [DbContext(typeof(StushbrDbContext))]
    [Migration("20231106195554_UpdateItemEntity")]
    partial class UpdateItemEntity
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.22")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Stushbr.Domain.Models.Clients.Client", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("SecondName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("Stushbr.Domain.Models.Clients.ClientItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("ClientId")
                        .HasColumnType("int");

                    b.Property<string>("Data")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsPaid")
                        .HasColumnType("bit");

                    b.Property<bool>("IsProcessed")
                        .HasColumnType("bit");

                    b.Property<int>("ItemId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("PaymentDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("PaymentSystemBillDueDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("PaymentSystemBillId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ProcessDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.HasIndex("ItemId");

                    b.ToTable("ClientItems");
                });

            modelBuilder.Entity("Stushbr.Domain.Models.Clients.TelegramClientItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ChannelName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ClientItemId")
                        .HasColumnType("int");

                    b.Property<string>("InviteLink")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LinkExpireDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("ClientItemId");

                    b.ToTable("TelegramClientItem");
                });

            modelBuilder.Entity("Stushbr.Domain.Models.Items.Item", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime?>("AvailableBefore")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("AvailableSince")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValue(new DateTime(2023, 11, 6, 19, 55, 53, 957, DateTimeKind.Utc).AddTicks(1869));

                    b.Property<string>("Data")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("ItemIdentifier")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ItemIdentifier")
                        .IsUnique();

                    b.ToTable("Items");
                });

            modelBuilder.Entity("Stushbr.Domain.Models.Items.TelegramItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("ItemId")
                        .HasColumnType("int");

                    b.Property<string>("SendPulseTemplateId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ItemId")
                        .IsUnique();

                    b.ToTable("TelegramItem");
                });

            modelBuilder.Entity("Stushbr.Domain.Models.Items.TelegramItemChannels", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<long>("ChannelId")
                        .HasColumnType("bigint");

                    b.Property<int?>("TelegramItemId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TelegramItemId");

                    b.ToTable("TelegramItemChannels");
                });

            modelBuilder.Entity("Stushbr.Domain.Models.Clients.ClientItem", b =>
                {
                    b.HasOne("Stushbr.Domain.Models.Clients.Client", "Client")
                        .WithMany("ClientItems")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Stushbr.Domain.Models.Items.Item", "Item")
                        .WithMany("ClientItems")
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Client");

                    b.Navigation("Item");
                });

            modelBuilder.Entity("Stushbr.Domain.Models.Clients.TelegramClientItem", b =>
                {
                    b.HasOne("Stushbr.Domain.Models.Clients.ClientItem", "ClientItem")
                        .WithMany("TelegramData")
                        .HasForeignKey("ClientItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ClientItem");
                });

            modelBuilder.Entity("Stushbr.Domain.Models.Items.TelegramItem", b =>
                {
                    b.HasOne("Stushbr.Domain.Models.Items.Item", "Item")
                        .WithOne("TelegramItem")
                        .HasForeignKey("Stushbr.Domain.Models.Items.TelegramItem", "ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Item");
                });

            modelBuilder.Entity("Stushbr.Domain.Models.Items.TelegramItemChannels", b =>
                {
                    b.HasOne("Stushbr.Domain.Models.Items.TelegramItem", null)
                        .WithMany("ChannelIds")
                        .HasForeignKey("TelegramItemId");
                });

            modelBuilder.Entity("Stushbr.Domain.Models.Clients.Client", b =>
                {
                    b.Navigation("ClientItems");
                });

            modelBuilder.Entity("Stushbr.Domain.Models.Clients.ClientItem", b =>
                {
                    b.Navigation("TelegramData");
                });

            modelBuilder.Entity("Stushbr.Domain.Models.Items.Item", b =>
                {
                    b.Navigation("ClientItems");

                    b.Navigation("TelegramItem");
                });

            modelBuilder.Entity("Stushbr.Domain.Models.Items.TelegramItem", b =>
                {
                    b.Navigation("ChannelIds");
                });
#pragma warning restore 612, 618
        }
    }
}