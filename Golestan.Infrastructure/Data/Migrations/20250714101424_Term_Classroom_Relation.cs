using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Golestan.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class Term_Classroom_Relation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Exams_ClassroomId",
                table: "Exams");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5adf2454-fd08-407c-bb5a-e01fca14ce90");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "89ee0464-092c-4b36-b965-61e2df23e16c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "911cf426-a988-4815-a7fd-bb3800db3e6d");

            migrationBuilder.AlterColumn<string>(
                name: "TermIdentifier",
                table: "Terms",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0e9243ee-e108-4cce-90a3-f430e7d5144d", null, "Admin", "ADMIN" },
                    { "1e2e300a-eeae-4deb-b042-46aa2d8aa17d", null, "Student", "STUDENT" },
                    { "8f1cbd1e-e6a0-42c5-b9dc-71b3e6185301", null, "Instructor", "INSTRUCTOR" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Terms_TermIdentifier",
                table: "Terms",
                column: "TermIdentifier",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Exams_ClassroomId",
                table: "Exams",
                column: "ClassroomId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Terms_TermIdentifier",
                table: "Terms");

            migrationBuilder.DropIndex(
                name: "IX_Exams_ClassroomId",
                table: "Exams");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0e9243ee-e108-4cce-90a3-f430e7d5144d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1e2e300a-eeae-4deb-b042-46aa2d8aa17d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8f1cbd1e-e6a0-42c5-b9dc-71b3e6185301");

            migrationBuilder.AlterColumn<string>(
                name: "TermIdentifier",
                table: "Terms",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "5adf2454-fd08-407c-bb5a-e01fca14ce90", null, "Instructor", "INSTRUCTOR" },
                    { "89ee0464-092c-4b36-b965-61e2df23e16c", null, "Admin", "ADMIN" },
                    { "911cf426-a988-4815-a7fd-bb3800db3e6d", null, "Student", "STUDENT" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Exams_ClassroomId",
                table: "Exams",
                column: "ClassroomId",
                unique: true);
        }
    }
}
