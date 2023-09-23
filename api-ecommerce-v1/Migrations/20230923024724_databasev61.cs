using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api_ecommerce_v1.Migrations
{
    /// <inheritdoc />
    public partial class databasev61 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "galerysId",
                table: "Product",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Galery",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    galery = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    productId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Galery", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Galery_Product_productId",
                        column: x => x.productId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Galery_productId",
                table: "Galery",
                column: "productId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Galery");

            migrationBuilder.DropColumn(
                name: "galerysId",
                table: "Product");
        }
    }
}
