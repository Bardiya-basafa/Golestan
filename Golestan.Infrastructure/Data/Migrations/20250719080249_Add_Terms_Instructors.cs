using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Golestan.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class Add_Terms_Instructors : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8a2024a4-1ff2-4b88-b9df-b4fdb6623fe5");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bb0bf3af-7ba5-4d88-b8a9-8c5bb8e25820");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "dc0ba99d-1b53-4cac-b61a-6218c6dfffe4");

            migrationBuilder.AddColumn<int>(
                name: "InstructorId",
                table: "Terms",
                type: "int",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "04a81f0f-5e63-4eeb-a425-ed04dda8206a", null, "Instructor", "INSTRUCTOR" },
                    { "245cb452-402f-4fc1-a3b0-4e42cce5bfd4", null, "Student", "STUDENT" },
                    { "82741709-3097-4ba4-ba0f-7c618a7e1b75", null, "Admin", "ADMIN" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Terms_InstructorId",
                table: "Terms",
                column: "InstructorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Terms_Instructors_InstructorId",
                table: "Terms",
                column: "InstructorId",
                principalTable: "Instructors",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Terms_Instructors_InstructorId",
                table: "Terms");

            migrationBuilder.DropIndex(
                name: "IX_Terms_InstructorId",
                table: "Terms");

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

            migrationBuilder.DropColumn(
                name: "InstructorId",
                table: "Terms");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "8a2024a4-1ff2-4b88-b9df-b4fdb6623fe5", null, "Student", "STUDENT" },
                    { "bb0bf3af-7ba5-4d88-b8a9-8c5bb8e25820", null, "Instructor", "INSTRUCTOR" },
                    { "dc0ba99d-1b53-4cac-b61a-6218c6dfffe4", null, "Admin", "ADMIN" }
                });
        }
    }
}
