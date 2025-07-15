using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Golestan.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Terms_TermIdentifier",
                table: "Terms");

            migrationBuilder.DropIndex(
                name: "IX_Exams_TermId",
                table: "Exams");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0e9243ee-e108-4cce-90a3-f430e7d5144d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1e2e300a-eeae-4deb-b042-46aa2d8aa17d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8f1cbd1e-e6a0-42c5-b9dc-71b3e6185301");

            migrationBuilder.AlterColumn<string>(
                name: "TermIdentifier",
                table: "Terms",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "272ab21e-bed1-42ea-baea-5599df3cfe27", null, "Instructor", "INSTRUCTOR" },
                    { "a5977bd5-e4e7-4b84-9f01-dd78ca3d1ae8", null, "Student", "STUDENT" },
                    { "c18cbb10-bffb-4b44-b01a-72979968077c", null, "Admin", "ADMIN" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Exams_TermId",
                table: "Exams",
                column: "TermId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Exams_TermId",
                table: "Exams");

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

            migrationBuilder.AlterColumn<string>(
                name: "TermIdentifier",
                table: "Terms",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0e9243ee-e108-4cce-90a3-f430e7d5144d", null, "Admin", "ADMIN" },
                    { "1e2e300a-eeae-4deb-b042-46aa2d8aa17d", null, "Student", "STUDENT" },
                    { "8f1cbd1e-e6a0-42c5-b9dc-71b3e6185301", null, "Instructor", "INSTRUCTOR" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Terms_TermIdentifier",
                table: "Terms",
                column: "TermIdentifier",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Exams_TermId",
                table: "Exams",
                column: "TermId",
                unique: true);
        }
    }
}
