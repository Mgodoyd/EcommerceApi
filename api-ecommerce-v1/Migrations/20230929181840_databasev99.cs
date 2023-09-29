using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api_ecommerce_v1.Migrations
{
    /// <inheritdoc />
    public partial class databasev99 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sales_NSale_nsaleId",
                table: "Sales");

            migrationBuilder.DropIndex(
                name: "IX_Sales_nsaleId",
                table: "Sales");

            migrationBuilder.AddColumn<int>(
                name: "SalesId",
                table: "NSale",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_NSale_SalesId",
                table: "NSale",
                column: "SalesId");

            migrationBuilder.AddForeignKey(
                name: "FK_NSale_Sales_SalesId",
                table: "NSale",
                column: "SalesId",
                principalTable: "Sales",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NSale_Sales_SalesId",
                table: "NSale");

            migrationBuilder.DropIndex(
                name: "IX_NSale_SalesId",
                table: "NSale");

            migrationBuilder.DropColumn(
                name: "SalesId",
                table: "NSale");

            migrationBuilder.CreateIndex(
                name: "IX_Sales_nsaleId",
                table: "Sales",
                column: "nsaleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_NSale_nsaleId",
                table: "Sales",
                column: "nsaleId",
                principalTable: "NSale",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
