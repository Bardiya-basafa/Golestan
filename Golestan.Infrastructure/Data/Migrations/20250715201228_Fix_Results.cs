using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Golestan.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class Fix_Results : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ExamResults_CourseId",
                table: "ExamResults");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4c226d24-bd36-4709-b79a-b0298ee2e1d4");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4c5b0241-76c7-46d3-8946-f43c669eb91d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bae8e4ab-f91f-49bf-abb1-92ba89d99341");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1969af0a-576a-41ff-9c0b-efb6898dff68", null, "Admin", "ADMIN" },
                    { "20e81cc9-d329-422f-abef-f24d67bb58c0", null, "Instructor", "INSTRUCTOR" },
                    { "4508e9c0-c0b7-4bd7-a379-1d2c08a12ef7", null, "Student", "STUDENT" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExamResults_CourseId",
                table: "ExamResults",
                column: "CourseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ExamResults_CourseId",
                table: "ExamResults");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1969af0a-576a-41ff-9c0b-efb6898dff68");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "20e81cc9-d329-422f-abef-f24d67bb58c0");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4508e9c0-c0b7-4bd7-a379-1d2c08a12ef7");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "4c226d24-bd36-4709-b79a-b0298ee2e1d4", null, "Instructor", "INSTRUCTOR" },
                    { "4c5b0241-76c7-46d3-8946-f43c669eb91d", null, "Student", "STUDENT" },
                    { "bae8e4ab-f91f-49bf-abb1-92ba89d99341", null, "Admin", "ADMIN" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExamResults_CourseId",
                table: "ExamResults",
                column: "CourseId",
                unique: true);
        }
    }
}
