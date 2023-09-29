using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api_ecommerce_v1.Migrations
{
    /// <inheritdoc />
    public partial class databasev95 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NSaleId",
                table: "User",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "userId",
                table: "NSale",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_User_NSaleId",
                table: "User",
                column: "NSaleId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_NSale_NSaleId",
                table: "User",
                column: "NSaleId",
                principalTable: "NSale",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_NSale_NSaleId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_NSaleId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "NSaleId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "userId",
                table: "NSale");
        }
    }
}
