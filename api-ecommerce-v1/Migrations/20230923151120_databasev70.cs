using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api_ecommerce_v1.Migrations
{
    /// <inheritdoc />
    public partial class databasev70 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Galery_productId",
                table: "Galery");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
