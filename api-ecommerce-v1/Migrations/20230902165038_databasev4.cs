using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api_ecommerce_v1.Migrations
{
    /// <inheritdoc />
    public partial class databasev4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Telefono",
                table: "Clientes",
                newName: "phone");

            migrationBuilder.RenameColumn(
                name: "Apellido",
                table: "Clientes",
                newName: "lastname");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "phone",
                table: "Clientes",
                newName: "Telefono");

            migrationBuilder.RenameColumn(
                name: "lastname",
                table: "Clientes",
                newName: "Apellido");
        }
    }
}
