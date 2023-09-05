using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api_ecommerce_v1.Migrations
{
    /// <inheritdoc />
    public partial class databasev7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Telefono",
                table: "Administrador");

            migrationBuilder.RenameColumn(
                name: "Profile",
                table: "Clientes",
                newName: "profile");

            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Clientes",
                newName: "password");

            migrationBuilder.RenameColumn(
                name: "Nit",
                table: "Clientes",
                newName: "nit");

            migrationBuilder.RenameColumn(
                name: "Fbirth",
                table: "Clientes",
                newName: "fbirth");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Clientes",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "Country",
                table: "Clientes",
                newName: "country");

            migrationBuilder.RenameColumn(
                name: "Rol",
                table: "Administrador",
                newName: "rol");

            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Administrador",
                newName: "password");

            migrationBuilder.RenameColumn(
                name: "Nit",
                table: "Administrador",
                newName: "nit");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Administrador",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "Apellido",
                table: "Administrador",
                newName: "lastname");

            migrationBuilder.AlterColumn<string>(
                name: "password",
                table: "Administrador",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<int>(
                name: "phone",
                table: "Administrador",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "phone",
                table: "Administrador");

            migrationBuilder.RenameColumn(
                name: "profile",
                table: "Clientes",
                newName: "Profile");

            migrationBuilder.RenameColumn(
                name: "password",
                table: "Clientes",
                newName: "Password");

            migrationBuilder.RenameColumn(
                name: "nit",
                table: "Clientes",
                newName: "Nit");

            migrationBuilder.RenameColumn(
                name: "fbirth",
                table: "Clientes",
                newName: "Fbirth");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "Clientes",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "country",
                table: "Clientes",
                newName: "Country");

            migrationBuilder.RenameColumn(
                name: "rol",
                table: "Administrador",
                newName: "Rol");

            migrationBuilder.RenameColumn(
                name: "password",
                table: "Administrador",
                newName: "Password");

            migrationBuilder.RenameColumn(
                name: "nit",
                table: "Administrador",
                newName: "Nit");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "Administrador",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "lastname",
                table: "Administrador",
                newName: "Apellido");

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "Administrador",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AddColumn<int>(
                name: "Telefono",
                table: "Administrador",
                type: "int",
                maxLength: 50,
                nullable: false,
                defaultValue: 0);
        }
    }
}
