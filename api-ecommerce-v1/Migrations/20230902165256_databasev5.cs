using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api_ecommerce_v1.Migrations
{
    /// <inheritdoc />
    public partial class databasev5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Perfil",
                table: "Clientes",
                newName: "gender");

            migrationBuilder.RenameColumn(
                name: "Pais",
                table: "Clientes",
                newName: "Profile");

            migrationBuilder.RenameColumn(
                name: "Genero",
                table: "Clientes",
                newName: "Fbirth");

            migrationBuilder.RenameColumn(
                name: "Fnacimiento",
                table: "Clientes",
                newName: "Country");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "gender",
                table: "Clientes",
                newName: "Perfil");

            migrationBuilder.RenameColumn(
                name: "Profile",
                table: "Clientes",
                newName: "Pais");

            migrationBuilder.RenameColumn(
                name: "Fbirth",
                table: "Clientes",
                newName: "Genero");

            migrationBuilder.RenameColumn(
                name: "Country",
                table: "Clientes",
                newName: "Fnacimiento");
        }
    }
}
