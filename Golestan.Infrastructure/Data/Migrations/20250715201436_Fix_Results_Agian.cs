using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Golestan.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class Fix_Results_Agian : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ExamResults_SectionId",
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
                    { "8a2024a4-1ff2-4b88-b9df-b4fdb6623fe5", null, "Student", "STUDENT" },
                    { "bb0bf3af-7ba5-4d88-b8a9-8c5bb8e25820", null, "Instructor", "INSTRUCTOR" },
                    { "dc0ba99d-1b53-4cac-b61a-6218c6dfffe4", null, "Admin", "ADMIN" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExamResults_SectionId",
                table: "ExamResults",
                column: "SectionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ExamResults_SectionId",
                table: "ExamResults");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8a2024a4-1ff2-4b88-b9df-b4fdb6623fe5");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bb0bf3af-7ba5-4d88-b8a9-8c5bb8e25820");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "dc0ba99d-1b53-4cac-b61a-6218c6dfffe4");

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
                name: "IX_ExamResults_SectionId",
                table: "ExamResults",
                column: "SectionId",
                unique: true);
        }
    }
}
