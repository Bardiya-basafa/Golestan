using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Golestan.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class Fix_Examresult : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ExamResults_TermId",
                table: "ExamResults");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6c0bcd27-5d43-41b9-b72c-80f0b7e6af84");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cc83737e-2bb6-4a94-b2b3-ebe8e0978488");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "edf0fb4f-a42a-41ec-8390-dc4e437546b1");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0b8c7de4-b24a-4c49-9444-0f38dd73d0d5", null, "Admin", "ADMIN" },
                    { "f04c3b2a-bb9d-446c-880d-176167e70b4a", null, "Instructor", "INSTRUCTOR" },
                    { "fc8bd2d3-a82e-4ad4-8541-820b58bbd6ae", null, "Student", "STUDENT" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExamResults_TermId",
                table: "ExamResults",
                column: "TermId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ExamResults_TermId",
                table: "ExamResults");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0b8c7de4-b24a-4c49-9444-0f38dd73d0d5");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f04c3b2a-bb9d-446c-880d-176167e70b4a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fc8bd2d3-a82e-4ad4-8541-820b58bbd6ae");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "6c0bcd27-5d43-41b9-b72c-80f0b7e6af84", null, "Instructor", "INSTRUCTOR" },
                    { "cc83737e-2bb6-4a94-b2b3-ebe8e0978488", null, "Admin", "ADMIN" },
                    { "edf0fb4f-a42a-41ec-8390-dc4e437546b1", null, "Student", "STUDENT" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExamResults_TermId",
                table: "ExamResults",
                column: "TermId",
                unique: true);
        }
    }
}
