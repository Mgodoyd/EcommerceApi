using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api_ecommerce_v1.Migrations
{
    /// <inheritdoc />
    public partial class databasev59 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Config_Category_categoryId",
                table: "Config");

            migrationBuilder.DropIndex(
                name: "IX_Config_categoryId",
                table: "Config");

            migrationBuilder.DropColumn(
                name: "categoryId",
                table: "Config");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "categoryId",
                table: "Config",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Config_categoryId",
                table: "Config",
                column: "categoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Config_Category_categoryId",
                table: "Config",
                column: "categoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
