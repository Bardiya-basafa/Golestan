using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Golestan.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Fix_FullName_Property : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "116530df-141b-4564-9bcc-d1d7694e8dd6");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "359c6fd7-7f38-422a-bb1c-c93c4c36cb21");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "62944b64-4718-4f1a-99c9-b7ae32dd728a");

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "Students",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "Instructors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0c8b7909-9ba4-4333-aa8c-1bc3a1d353bf", null, "Instructor", "INSTRUCTOR" },
                    { "c0c11ac0-41f0-4628-8818-efa9a1dfa286", null, "Admin", "ADMIN" },
                    { "f3ffcd36-1ca0-4ebb-a71e-786f665b9e1e", null, "Student", "STUDENT" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0c8b7909-9ba4-4333-aa8c-1bc3a1d353bf");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c0c11ac0-41f0-4628-8818-efa9a1dfa286");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f3ffcd36-1ca0-4ebb-a71e-786f665b9e1e");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "Instructors");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "116530df-141b-4564-9bcc-d1d7694e8dd6", null, "Admin", "ADMIN" },
                    { "359c6fd7-7f38-422a-bb1c-c93c4c36cb21", null, "Student", "STUDENT" },
                    { "62944b64-4718-4f1a-99c9-b7ae32dd728a", null, "Instructor", "INSTRUCTOR" }
                });
        }
    }
}
