using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stushbr.Data.DataAccess.Sql.Migrations
{
    public partial class UpdateItemsEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TelegramItem_Items_ItemId",
                table: "TelegramItem");

            migrationBuilder.DropTable(
                name: "TelegramItemChannels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TelegramItem",
                table: "TelegramItem");

            migrationBuilder.RenameTable(
                name: "TelegramItem",
                newName: "TelegramItems");

            migrationBuilder.RenameIndex(
                name: "IX_TelegramItem_ItemId",
                table: "TelegramItems",
                newName: "IX_TelegramItems_ItemId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AvailableSince",
                table: "Items",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 11, 11, 20, 9, 16, 19, DateTimeKind.Utc).AddTicks(740),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 11, 6, 19, 55, 53, 957, DateTimeKind.Utc).AddTicks(1869));

            migrationBuilder.AddPrimaryKey(
                name: "PK_TelegramItems",
                table: "TelegramItems",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Channels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChannelId = table.Column<long>(type: "bigint", nullable: false),
                    TelegramItemId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Channels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Channels_TelegramItems_TelegramItemId",
                        column: x => x.TelegramItemId,
                        principalTable: "TelegramItems",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Channels_TelegramItemId",
                table: "Channels",
                column: "TelegramItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_TelegramItems_Items_ItemId",
                table: "TelegramItems",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TelegramItems_Items_ItemId",
                table: "TelegramItems");

            migrationBuilder.DropTable(
                name: "Channels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TelegramItems",
                table: "TelegramItems");

            migrationBuilder.RenameTable(
                name: "TelegramItems",
                newName: "TelegramItem");

            migrationBuilder.RenameIndex(
                name: "IX_TelegramItems_ItemId",
                table: "TelegramItem",
                newName: "IX_TelegramItem_ItemId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AvailableSince",
                table: "Items",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 11, 6, 19, 55, 53, 957, DateTimeKind.Utc).AddTicks(1869),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 11, 11, 20, 9, 16, 19, DateTimeKind.Utc).AddTicks(740));

            migrationBuilder.AddPrimaryKey(
                name: "PK_TelegramItem",
                table: "TelegramItem",
                column: "Id");

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
                name: "IX_TelegramItemChannels_TelegramItemId",
                table: "TelegramItemChannels",
                column: "TelegramItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_TelegramItem_Items_ItemId",
                table: "TelegramItem",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
