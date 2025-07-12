using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Golestan.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class Fixing_Term : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "27a8a935-288c-4aa1-a42e-93a0212ecbda");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "91f455b1-0956-4c1b-a43b-774596aa309f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "be781783-ee49-407f-86fd-d42c4afc2b38");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Terms");

            migrationBuilder.AlterColumn<string>(
                name: "Major",
                table: "Faculties",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "602c0d54-2831-4bf0-bec8-d256e4edb882", null, "Instructor", "INSTRUCTOR" },
                    { "9b854781-f642-492e-a80f-90ecaaf5fe89", null, "Student", "STUDENT" },
                    { "a909f6e7-6d87-437d-bac6-ed1d4b19baf3", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "602c0d54-2831-4bf0-bec8-d256e4edb882");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9b854781-f642-492e-a80f-90ecaaf5fe89");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a909f6e7-6d87-437d-bac6-ed1d4b19baf3");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Terms",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "Major",
                table: "Faculties",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(25)",
                oldMaxLength: 25);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "27a8a935-288c-4aa1-a42e-93a0212ecbda", null, "Student", "STUDENT" },
                    { "91f455b1-0956-4c1b-a43b-774596aa309f", null, "Admin", "ADMIN" },
                    { "be781783-ee49-407f-86fd-d42c4afc2b38", null, "Instructor", "INSTRUCTOR" }
                });
        }
    }
}
