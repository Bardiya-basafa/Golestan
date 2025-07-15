using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Golestan.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class Fix_Course : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "272ab21e-bed1-42ea-baea-5599df3cfe27");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a5977bd5-e4e7-4b84-9f01-dd78ca3d1ae8");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c18cbb10-bffb-4b44-b01a-72979968077c");

            migrationBuilder.AddColumn<bool>(
                name: "ExamIsSet",
                table: "Courses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "6c0bcd27-5d43-41b9-b72c-80f0b7e6af84", null, "Instructor", "INSTRUCTOR" },
                    { "cc83737e-2bb6-4a94-b2b3-ebe8e0978488", null, "Admin", "ADMIN" },
                    { "edf0fb4f-a42a-41ec-8390-dc4e437546b1", null, "Student", "STUDENT" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.DropColumn(
                name: "ExamIsSet",
                table: "Courses");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "272ab21e-bed1-42ea-baea-5599df3cfe27", null, "Instructor", "INSTRUCTOR" },
                    { "a5977bd5-e4e7-4b84-9f01-dd78ca3d1ae8", null, "Student", "STUDENT" },
                    { "c18cbb10-bffb-4b44-b01a-72979968077c", null, "Admin", "ADMIN" }
                });
        }
    }
}
