using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EDUBackEnd.Migrations
{
    /// <inheritdoc />
    public partial class SchoolClassConnAddedToStudent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Class",
                table: "Students");

            migrationBuilder.RenameColumn(
                name: "Year",
                table: "Students",
                newName: "SchoolClassId");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "SchoolClasses",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "SchoolClasses",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Students_SchoolClassId",
                table: "Students",
                column: "SchoolClassId");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_SchoolClasses_SchoolClassId",
                table: "Students",
                column: "SchoolClassId",
                principalTable: "SchoolClasses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_SchoolClasses_SchoolClassId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_SchoolClassId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "SchoolClasses");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "SchoolClasses");

            migrationBuilder.RenameColumn(
                name: "SchoolClassId",
                table: "Students",
                newName: "Year");

            migrationBuilder.AddColumn<string>(
                name: "Class",
                table: "Students",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
