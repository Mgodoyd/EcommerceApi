using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api_ecommerce_v1.Migrations
{
    /// <inheritdoc />
    public partial class databasev113 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_NSale_NSaleId",
                table: "Product");

            migrationBuilder.DropForeignKey(
                name: "FK_User_NSale_NSaleId",
                table: "User");

            migrationBuilder.DropTable(
                name: "NSale");

            migrationBuilder.DropIndex(
                name: "IX_User_NSaleId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_Product_NSaleId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "NSaleId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "NSaleId",
                table: "Product");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NSaleId",
                table: "User",
                type: "int",
                nullable: true);

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
                    SalesId = table.Column<int>(type: "int", nullable: false),
                    amount = table.Column<int>(type: "int", nullable: false),
                    createdDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    productId = table.Column<int>(type: "int", nullable: false),
                    subtotal = table.Column<int>(type: "int", nullable: false),
                    userId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NSale", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NSale_Sales_SalesId",
                        column: x => x.SalesId,
                        principalTable: "Sales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_User_NSaleId",
                table: "User",
                column: "NSaleId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_NSaleId",
                table: "Product",
                column: "NSaleId");

            migrationBuilder.CreateIndex(
                name: "IX_NSale_SalesId",
                table: "NSale",
                column: "SalesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_NSale_NSaleId",
                table: "Product",
                column: "NSaleId",
                principalTable: "NSale",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_User_NSale_NSaleId",
                table: "User",
                column: "NSaleId",
                principalTable: "NSale",
                principalColumn: "Id");
        }
    }
}
