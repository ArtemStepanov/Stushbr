using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stushbr.Data.DataAccess.Sql.Migrations
{
    public partial class UpdateItemEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ItemIdentifier",
                table: "Items",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AvailableSince",
                table: "Items",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 11, 6, 19, 55, 53, 957, DateTimeKind.Utc).AddTicks(1869),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 9, 29, 21, 0, 16, 702, DateTimeKind.Utc).AddTicks(6783));

            migrationBuilder.CreateIndex(
                name: "IX_Items_ItemIdentifier",
                table: "Items",
                column: "ItemIdentifier",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Items_ItemIdentifier",
                table: "Items");

            migrationBuilder.AlterColumn<string>(
                name: "ItemIdentifier",
                table: "Items",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AvailableSince",
                table: "Items",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 9, 29, 21, 0, 16, 702, DateTimeKind.Utc).AddTicks(6783),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 11, 6, 19, 55, 53, 957, DateTimeKind.Utc).AddTicks(1869));
        }
    }
}
