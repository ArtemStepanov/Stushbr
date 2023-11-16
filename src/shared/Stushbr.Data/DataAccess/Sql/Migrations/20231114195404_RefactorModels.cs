using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stushbr.Data.DataAccess.Sql.Migrations
{
    public partial class RefactorModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Create a new temporary table without the identity column
            migrationBuilder.Sql(@"
SELECT TelegramItemId, ChannelId
INTO Temp_Channels
FROM Channels");

            // Drop the original table
            migrationBuilder.DropTable(name: "Channels");

            // Rename the temporary table to the original table's name
            migrationBuilder.Sql(@"EXEC sp_rename 'Temp_Channels', 'Channels';");

            migrationBuilder.DropForeignKey(
                name: "FK_ClientItems_Clients_ClientId",
                table: "ClientItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ClientItems_Items_ItemId",
                table: "ClientItems");

            migrationBuilder.DropForeignKey(
                name: "FK_TelegramClientItem_ClientItems_ClientItemId",
                table: "TelegramClientItem");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AvailableSince",
                table: "Items",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 11, 14, 19, 54, 4, 721, DateTimeKind.Utc).AddTicks(3813),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 11, 11, 20, 9, 16, 19, DateTimeKind.Utc).AddTicks(740));

            migrationBuilder.AlterColumn<int>(
                name: "TelegramItemId",
                table: "Channels",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Channels",
                table: "Channels",
                columns: new[] { "TelegramItemId", "ChannelId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Channels_TelegramItems_TelegramItemId",
                table: "Channels",
                column: "TelegramItemId",
                principalTable: "TelegramItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientItems_Clients_ClientId",
                table: "ClientItems",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientItems_Items_ItemId",
                table: "ClientItems",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TelegramClientItem_ClientItems_ClientItemId",
                table: "TelegramClientItem",
                column: "ClientItemId",
                principalTable: "ClientItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Create a new temporary table with the identity column
            migrationBuilder.Sql(@"
SELECT IDENTITY(int, 1, 1) AS Id, TelegramItemId, ChannelId
INTO Temp_Channels
FROM Channels");

            // Drop the original table
            migrationBuilder.DropTable(name: "Channels");

            // Rename the temporary table to the original table's name
            migrationBuilder.Sql(@"EXEC sp_rename 'Temp_Channels', 'Channels';");

            migrationBuilder.DropForeignKey(
                name: "FK_ClientItems_Clients_ClientId",
                table: "ClientItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ClientItems_Items_ItemId",
                table: "ClientItems");

            migrationBuilder.DropForeignKey(
                name: "FK_TelegramClientItem_ClientItems_ClientItemId",
                table: "TelegramClientItem");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AvailableSince",
                table: "Items",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 11, 11, 20, 9, 16, 19, DateTimeKind.Utc).AddTicks(740),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 11, 14, 19, 54, 4, 721, DateTimeKind.Utc).AddTicks(3813));

            migrationBuilder.AlterColumn<int>(
                name: "TelegramItemId",
                table: "Channels",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Channels",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Channels",
                table: "Channels",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Channels_TelegramItemId",
                table: "Channels",
                column: "TelegramItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Channels_TelegramItems_TelegramItemId",
                table: "Channels",
                column: "TelegramItemId",
                principalTable: "TelegramItems",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientItems_Clients_ClientId",
                table: "ClientItems",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientItems_Items_ItemId",
                table: "ClientItems",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TelegramClientItem_ClientItems_ClientItemId",
                table: "TelegramClientItem",
                column: "ClientItemId",
                principalTable: "ClientItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
