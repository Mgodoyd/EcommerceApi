using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api_ecommerce_v1.Migrations
{
    /// <inheritdoc />
    public partial class databasev93 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "sales",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "state",
                table: "Product");

            migrationBuilder.CreateTable(
                name: "Sales",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nsale = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    subtotal = table.Column<int>(type: "int", nullable: false),
                    envio_title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    envio_price = table.Column<int>(type: "int", nullable: false),
                    transaction = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    coupon = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    state = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    note = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    createdDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    userId = table.Column<int>(type: "int", nullable: false),
                    addressId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sales", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sales_Address_addressId",
                        column: x => x.addressId,
                        principalTable: "Address",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sales_User_userId",
                        column: x => x.userId,
                        principalTable: "User",
                        principalColumn: "Id",
                         onDelete: ReferentialAction.NoAction,
                         onUpdate: ReferentialAction.NoAction); 
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sales_addressId",
                table: "Sales",
                column: "addressId");

            migrationBuilder.CreateIndex(
                name: "IX_Sales_userId",
                table: "Sales",
                column: "userId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sales");

            migrationBuilder.AddColumn<int>(
                name: "sales",
                table: "Product",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "state",
                table: "Product",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
