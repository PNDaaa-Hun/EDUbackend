using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EDUBackEnd.Migrations
{
    /// <inheritdoc />
    public partial class MakingTeachAndStudentsAbsenceable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "Every");

            migrationBuilder.AddColumn<bool>(
                name: "IsAbsence",
                table: "Students",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsCancelled",
                table: "ScheduleEntries",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsTeacherAbsence",
                table: "ScheduleEntries",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ReplaceTeacherId",
                table: "ScheduleEntries",
                type: "int",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "CafeteriaStaff", null, "CafeteriaStaff", "CAFETERIASTAFF" },
                    { "Counselor", null, "Counselor", "COUNSELOR" },
                    { "HeadTeacher", null, "HeadTeacher", "HEADTEACHER" },
                    { "ITSupport", null, "ITSupport", "ITSUPPORT" },
                    { "Janitor", null, "Janitor", "JANITOR" },
                    { "Librarian", null, "Librarian", "LIBRARIAN" },
                    { "Nurse", null, "Nurse", "NURSE" },
                    { "Principal", null, "Principal", "PRINCIPAL" },
                    { "PrincipalAssistant", null, "PrincipalAssistant", "PRINCIPALASSISTANT" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleEntries_ReplaceTeacherId",
                table: "ScheduleEntries",
                column: "ReplaceTeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduleEntries_Teachers_ReplaceTeacherId",
                table: "ScheduleEntries",
                column: "ReplaceTeacherId",
                principalTable: "Teachers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScheduleEntries_Teachers_ReplaceTeacherId",
                table: "ScheduleEntries");

            migrationBuilder.DropIndex(
                name: "IX_ScheduleEntries_ReplaceTeacherId",
                table: "ScheduleEntries");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "CafeteriaStaff");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "Counselor");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "HeadTeacher");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ITSupport");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "Janitor");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "Librarian");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "Nurse");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "Principal");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "PrincipalAssistant");

            migrationBuilder.DropColumn(
                name: "IsAbsence",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "IsCancelled",
                table: "ScheduleEntries");

            migrationBuilder.DropColumn(
                name: "IsTeacherAbsence",
                table: "ScheduleEntries");

            migrationBuilder.DropColumn(
                name: "ReplaceTeacherId",
                table: "ScheduleEntries");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "Every", null, "Every", "EVERY" });
        }
    }
}
