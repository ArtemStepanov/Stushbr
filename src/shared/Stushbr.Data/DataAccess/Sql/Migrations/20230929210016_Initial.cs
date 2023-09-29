using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stushbr.Data.DataAccess.Sql.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SecondName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<double>(type: "float", nullable: false),
                    ItemIdentifier = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Data = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AvailableSince = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2023, 9, 29, 21, 0, 16, 702, DateTimeKind.Utc).AddTicks(6783)),
                    AvailableBefore = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClientItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    PaymentSystemBillId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentSystemBillDueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsPaid = table.Column<bool>(type: "bit", nullable: false),
                    IsProcessed = table.Column<bool>(type: "bit", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ProcessDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Data = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientItems_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientItems_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TelegramItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SendPulseTemplateId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ItemId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TelegramItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TelegramItem_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TelegramClientItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InviteLink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LinkExpireDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ChannelName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClientItemId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TelegramClientItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TelegramClientItem_ClientItems_ClientItemId",
                        column: x => x.ClientItemId,
                        principalTable: "ClientItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TelegramItemChannels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChannelId = table.Column<long>(type: "bigint", nullable: false),
                    TelegramItemId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TelegramItemChannels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TelegramItemChannels_TelegramItem_TelegramItemId",
                        column: x => x.TelegramItemId,
                        principalTable: "TelegramItem",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClientItems_ClientId",
                table: "ClientItems",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientItems_ItemId",
                table: "ClientItems",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TelegramClientItem_ClientItemId",
                table: "TelegramClientItem",
                column: "ClientItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TelegramItem_ItemId",
                table: "TelegramItem",
                column: "ItemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TelegramItemChannels_TelegramItemId",
                table: "TelegramItemChannels",
                column: "TelegramItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TelegramClientItem");

            migrationBuilder.DropTable(
                name: "TelegramItemChannels");

            migrationBuilder.DropTable(
                name: "ClientItems");

            migrationBuilder.DropTable(
                name: "TelegramItem");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "Items");
        }
    }
}
