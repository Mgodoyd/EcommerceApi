using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api_ecommerce_v1.Migrations
{
    /// <inheritdoc />
    public partial class databasev82 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "slug",
                table: "Product");

            migrationBuilder.RenameColumn(
                name: "stock",
                table: "Cart",
                newName: "amount");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "amount",
                table: "Cart",
                newName: "stock");

            migrationBuilder.AddColumn<string>(
                name: "slug",
                table: "Product",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }
    }
}
