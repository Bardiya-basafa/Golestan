using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Golestan.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class Fix_Roles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "04a81f0f-5e63-4eeb-a425-ed04dda8206a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "245cb452-402f-4fc1-a3b0-4e42cce5bfd4");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "82741709-3097-4ba4-ba0f-7c618a7e1b75");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "04a81f0f-5e63-4eeb-a425-ed04dda8206a", null, "Instructor", "INSTRUCTOR" },
                    { "245cb452-402f-4fc1-a3b0-4e42cce5bfd4", null, "Student", "STUDENT" },
                    { "82741709-3097-4ba4-ba0f-7c618a7e1b75", null, "Admin", "ADMIN" }
                });
        }
    }
}
