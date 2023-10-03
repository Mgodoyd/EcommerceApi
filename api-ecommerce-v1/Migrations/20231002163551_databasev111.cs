using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api_ecommerce_v1.Migrations
{
    /// <inheritdoc />
    public partial class databasev111 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NSale_Sales_SalesId",
                table: "NSale");

            migrationBuilder.AlterColumn<int>(
                name: "SalesId",
                table: "NSale",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_NSale_Sales_SalesId",
                table: "NSale",
                column: "SalesId",
                principalTable: "Sales",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NSale_Sales_SalesId",
                table: "NSale");

            migrationBuilder.AlterColumn<int>(
                name: "SalesId",
                table: "NSale",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_NSale_Sales_SalesId",
                table: "NSale",
                column: "SalesId",
                principalTable: "Sales",
                principalColumn: "Id");
        }
    }
}
