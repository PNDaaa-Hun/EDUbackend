using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EDUBackEnd.Migrations
{
    /// <inheritdoc />
    public partial class StudentFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_SchoolClasses_SchoolClassId",
                table: "Students");

            migrationBuilder.RenameColumn(
                name: "SchoolClassId",
                table: "Students",
                newName: "ClassId");

            migrationBuilder.RenameIndex(
                name: "IX_Students_SchoolClassId",
                table: "Students",
                newName: "IX_Students_ClassId");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_SchoolClasses_ClassId",
                table: "Students",
                column: "ClassId",
                principalTable: "SchoolClasses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_SchoolClasses_ClassId",
                table: "Students");

            migrationBuilder.RenameColumn(
                name: "ClassId",
                table: "Students",
                newName: "SchoolClassId");

            migrationBuilder.RenameIndex(
                name: "IX_Students_ClassId",
                table: "Students",
                newName: "IX_Students_SchoolClassId");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_SchoolClasses_SchoolClassId",
                table: "Students",
                column: "SchoolClassId",
                principalTable: "SchoolClasses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
