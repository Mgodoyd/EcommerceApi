using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api_ecommerce_v1.Migrations
{
    /// <inheritdoc />
    public partial class databasev86 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Cart_productId",
                table: "Cart",
                column: "productId");

            migrationBuilder.CreateIndex(
                name: "IX_Cart_userId",
                table: "Cart",
                column: "userId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cart_Product_productId",
                table: "Cart",
                column: "productId",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Cart_User_userId",
                table: "Cart",
                column: "userId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cart_Product_productId",
                table: "Cart");

            migrationBuilder.DropForeignKey(
                name: "FK_Cart_User_userId",
                table: "Cart");

            migrationBuilder.DropIndex(
                name: "IX_Cart_productId",
                table: "Cart");

            migrationBuilder.DropIndex(
                name: "IX_Cart_userId",
                table: "Cart");
        }
    }
}
