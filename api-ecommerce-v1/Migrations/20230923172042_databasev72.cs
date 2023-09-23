using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api_ecommerce_v1.Migrations
{
    /// <inheritdoc />
    public partial class databasev72 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_User_LoginId",
                table: "User");

            migrationBuilder.CreateIndex(
                name: "IX_User_LoginId",
                table: "User",
                column: "LoginId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_User_LoginId",
                table: "User");

            migrationBuilder.CreateIndex(
                name: "IX_User_LoginId",
                table: "User",
                column: "LoginId");
        }
    }
}
