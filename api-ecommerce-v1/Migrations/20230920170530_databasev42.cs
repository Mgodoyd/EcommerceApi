using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api_ecommerce_v1.Migrations
{
    /// <inheritdoc />
    public partial class databasev42 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_User_LoginId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_Product_inventoryId",
                table: "Product");

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "Inventory",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_User_LoginId",
                table: "User",
                column: "LoginId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_inventoryId",
                table: "Product",
                column: "inventoryId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_User_LoginId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_Product_inventoryId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Inventory");

            migrationBuilder.CreateIndex(
                name: "IX_User_LoginId",
                table: "User",
                column: "LoginId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Product_inventoryId",
                table: "Product",
                column: "inventoryId");
        }
    }
}
