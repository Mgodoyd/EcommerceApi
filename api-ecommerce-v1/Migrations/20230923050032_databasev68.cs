using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api_ecommerce_v1.Migrations
{
    /// <inheritdoc />
    public partial class databasev68 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "galerysId",
                table: "Product",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "productId",
                table: "Galery",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Galery_productId",
                table: "Galery",
                column: "productId",
                unique: false);

            migrationBuilder.AddForeignKey(
                name: "FK_Galery_Product_productId",
                table: "Galery",
                column: "productId",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Galery_Product_productId",
                table: "Galery");

            migrationBuilder.DropIndex(
                name: "IX_Galery_productId",
                table: "Galery");

            migrationBuilder.DropColumn(
                name: "galerysId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "productId",
                table: "Galery");
        }
    }
}
