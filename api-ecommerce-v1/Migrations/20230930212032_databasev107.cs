using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api_ecommerce_v1.Migrations
{
    /// <inheritdoc />
    public partial class databasev107 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contact_User_userId",
                table: "Contact");

            migrationBuilder.DropIndex(
                name: "IX_Contact_userId",
                table: "Contact");

            migrationBuilder.DropColumn(
                name: "userId",
                table: "Contact");

            migrationBuilder.AddColumn<string>(
                name: "customer",
                table: "Contact",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "customer",
                table: "Contact");

            migrationBuilder.AddColumn<int>(
                name: "userId",
                table: "Contact",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Contact_userId",
                table: "Contact",
                column: "userId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contact_User_userId",
                table: "Contact",
                column: "userId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
