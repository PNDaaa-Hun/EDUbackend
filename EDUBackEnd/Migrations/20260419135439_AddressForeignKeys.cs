using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EDUBackEnd.Migrations
{
    /// <inheritdoc />
    public partial class AddressForeignKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AddressId",
                table: "Teachers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AddressId",
                table: "Admins",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_AddressId",
                table: "Teachers",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Admins_AddressId",
                table: "Admins",
                column: "AddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_Admins_Addresses_AddressId",
                table: "Admins",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Teachers_Addresses_AddressId",
                table: "Teachers",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Admins_Addresses_AddressId",
                table: "Admins");

            migrationBuilder.DropForeignKey(
                name: "FK_Teachers_Addresses_AddressId",
                table: "Teachers");

            migrationBuilder.DropIndex(
                name: "IX_Teachers_AddressId",
                table: "Teachers");

            migrationBuilder.DropIndex(
                name: "IX_Admins_AddressId",
                table: "Admins");

            migrationBuilder.DropColumn(
                name: "AddressId",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "AddressId",
                table: "Admins");
        }
    }
}
