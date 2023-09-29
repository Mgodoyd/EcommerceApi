using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api_ecommerce_v1.Migrations
{
    /// <inheritdoc />
    public partial class databasev94 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "nsale",
                table: "Sales");

            migrationBuilder.AddColumn<int>(
                name: "nsaleId",
                table: "Sales",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NSaleId",
                table: "Product",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "NSale",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    subtotal = table.Column<int>(type: "int", nullable: false),
                    amount = table.Column<int>(type: "int", nullable: false),
                    createdDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    productId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NSale", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sales_nsaleId",
                table: "Sales",
                column: "nsaleId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_NSaleId",
                table: "Product",
                column: "NSaleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_NSale_NSaleId",
                table: "Product",
                column: "NSaleId",
                principalTable: "NSale",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_NSale_nsaleId",
                table: "Sales",
                column: "nsaleId",
                principalTable: "NSale",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_NSale_NSaleId",
                table: "Product");

            migrationBuilder.DropForeignKey(
                name: "FK_Sales_NSale_nsaleId",
                table: "Sales");

            migrationBuilder.DropTable(
                name: "NSale");

            migrationBuilder.DropIndex(
                name: "IX_Sales_nsaleId",
                table: "Sales");

            migrationBuilder.DropIndex(
                name: "IX_Product_NSaleId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "nsaleId",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "NSaleId",
                table: "Product");

            migrationBuilder.AddColumn<string>(
                name: "nsale",
                table: "Sales",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }
    }
}
