using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Golestan.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Fix_Missed_Relations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.RenameColumn(
                name: "Badge",
                table: "Faculties",
                newName: "Budget");

            migrationBuilder.AddColumn<int>(
                name: "InstructorId",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StudentId",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0e84738a-abe5-4050-852c-3233f2968455", null, "Admin", "ADMIN" },
                    { "676b7cc8-c4c8-4f18-bf7d-a8b3844696b9", null, "Student", "STUDENT" },
                    { "f8d0f29a-e727-4ad8-ad75-ce0b16e95eec", null, "Instructor", "INSTRUCTOR" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0e84738a-abe5-4050-852c-3233f2968455");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "676b7cc8-c4c8-4f18-bf7d-a8b3844696b9");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f8d0f29a-e727-4ad8-ad75-ce0b16e95eec");

            migrationBuilder.DropColumn(
                name: "InstructorId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "Budget",
                table: "Faculties",
                newName: "Badge");

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
    }
}
