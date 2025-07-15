using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Golestan.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class Gpa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.AddColumn<decimal>(
                name: "Gpa",
                table: "Students",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "4c226d24-bd36-4709-b79a-b0298ee2e1d4", null, "Instructor", "INSTRUCTOR" },
                    { "4c5b0241-76c7-46d3-8946-f43c669eb91d", null, "Student", "STUDENT" },
                    { "bae8e4ab-f91f-49bf-abb1-92ba89d99341", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.DropColumn(
                name: "Gpa",
                table: "Students");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0b8c7de4-b24a-4c49-9444-0f38dd73d0d5", null, "Admin", "ADMIN" },
                    { "f04c3b2a-bb9d-446c-880d-176167e70b4a", null, "Instructor", "INSTRUCTOR" },
                    { "fc8bd2d3-a82e-4ad4-8541-820b58bbd6ae", null, "Student", "STUDENT" }
                });
        }
    }
}
