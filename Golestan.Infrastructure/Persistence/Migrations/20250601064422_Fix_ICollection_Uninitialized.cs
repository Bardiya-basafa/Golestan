using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Golestan.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Fix_ICollection_Uninitialized : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5cd3c0a9-62a4-4256-8a5c-dac56da94355");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "62427d4a-9bc7-4184-98f7-fa051ebd3f49");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a0bded25-26dd-4d62-a310-bb4f03ccf9ae");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "abea5ff8-4e2d-4cfa-a1b2-07a58824e699", null, "Student", "STUDENT" },
                    { "bbaaaa75-88ea-4716-b6f6-7945188e6a3c", null, "Instructor", "INSTRUCTOR" },
                    { "d84a46a9-361e-4e67-a4aa-2fc7ac63f0c7", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "abea5ff8-4e2d-4cfa-a1b2-07a58824e699");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bbaaaa75-88ea-4716-b6f6-7945188e6a3c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d84a46a9-361e-4e67-a4aa-2fc7ac63f0c7");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "5cd3c0a9-62a4-4256-8a5c-dac56da94355", null, "Instructor", "INSTRUCTOR" },
                    { "62427d4a-9bc7-4184-98f7-fa051ebd3f49", null, "Student", "STUDENT" },
                    { "a0bded25-26dd-4d62-a310-bb4f03ccf9ae", null, "Admin", "ADMIN" }
                });
        }
    }
}
