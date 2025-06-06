using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Golestan.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Fix_Classroom_Property : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Classrooms",
                newName: "ClassNumber");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.RenameColumn(
                name: "ClassNumber",
                table: "Classrooms",
                newName: "Name");

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
    }
}
