using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api_ecommerce_v1.Migrations
{
    /// <inheritdoc />
    public partial class databasev16 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LoginId",
                table: "Clientes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "rol",
                table: "Clientes",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_LoginId",
                table: "Clientes",
                column: "LoginId");

            migrationBuilder.AddForeignKey(
                name: "FK_Clientes_Login_LoginId",
                table: "Clientes",
                column: "LoginId",
                principalTable: "Login",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clientes_Login_LoginId",
                table: "Clientes");

            migrationBuilder.DropIndex(
                name: "IX_Clientes_LoginId",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "LoginId",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "rol",
                table: "Clientes");
        }
    }
}
