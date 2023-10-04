using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api_ecommerce_v1.Migrations
{
    /// <inheritdoc />
    public partial class databasev117 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_NSales_NSaleId",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_Product_NSaleId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "NSaleId",
                table: "Product");

            migrationBuilder.CreateIndex(
                name: "IX_NSales_productId",
                table: "NSales",
                column: "productId");

            migrationBuilder.AddForeignKey(
                name: "FK_NSales_Product_productId",
                table: "NSales",
                column: "productId",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NSales_Product_productId",
                table: "NSales");

            migrationBuilder.DropIndex(
                name: "IX_NSales_productId",
                table: "NSales");

            migrationBuilder.AddColumn<int>(
                name: "NSaleId",
                table: "Product",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Product_NSaleId",
                table: "Product",
                column: "NSaleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_NSales_NSaleId",
                table: "Product",
                column: "NSaleId",
                principalTable: "NSales",
                principalColumn: "Id");
        }
    }
}
