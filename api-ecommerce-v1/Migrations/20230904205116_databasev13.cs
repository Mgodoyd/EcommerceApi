using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api_ecommerce_v1.Migrations
{
    /// <inheritdoc />
    public partial class databasev13 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LoginId",
                table: "Administrador",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Administrador_LoginId",
                table: "Administrador",
                column: "LoginId");

            migrationBuilder.AddForeignKey(
                name: "FK_Administrador_Login_LoginId",
                table: "Administrador",
                column: "LoginId",
                principalTable: "Login",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Administrador_Login_LoginId",
                table: "Administrador");

            migrationBuilder.DropIndex(
                name: "IX_Administrador_LoginId",
                table: "Administrador");

            migrationBuilder.DropColumn(
                name: "LoginId",
                table: "Administrador");
        }
    }
}
