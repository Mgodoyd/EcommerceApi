using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api_ecommerce_v1.Migrations
{
    /// <inheritdoc />
    public partial class databasev98 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sales_NSale_nsaleId",
                table: "Sales");

            migrationBuilder.AlterColumn<int>(
                name: "nsaleId",
                table: "Sales",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_NSale_nsaleId",
                table: "Sales",
                column: "nsaleId",
                principalTable: "NSale",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sales_NSale_nsaleId",
                table: "Sales");

            migrationBuilder.AlterColumn<int>(
                name: "nsaleId",
                table: "Sales",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_NSale_nsaleId",
                table: "Sales",
                column: "nsaleId",
                principalTable: "NSale",
                principalColumn: "Id");
        }
    }
}
