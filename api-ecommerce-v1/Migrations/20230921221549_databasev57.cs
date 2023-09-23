using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api_ecommerce_v1.Migrations
{
    /// <inheritdoc />
    public partial class databasev57 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "title",
                table: "Category",
                newName: "titles");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "titles",
                table: "Category",
                newName: "title");
        }
    }
}
