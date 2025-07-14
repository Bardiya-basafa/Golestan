using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Golestan.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class Fix_Instructor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1c3fcabb-69f9-41ff-9fec-d99f551e53ab");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7a1db28c-9966-41f2-a28e-eac4165c32bd");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a9942bc2-9371-4fb5-86f4-9ec553103261");

            migrationBuilder.AddColumn<string>(
                name: "InstructorNumber",
                table: "Instructors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "AppMessage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AppUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AppUserId1 = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsSuccess = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppMessage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppMessage_Users_AppUserId1",
                        column: x => x.AppUserId1,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "27cea1a7-0dac-4e78-af96-a9d219fe2223", null, "Student", "STUDENT" },
                    { "73a3657b-bc9d-4c64-be7b-3a827750a25d", null, "Instructor", "INSTRUCTOR" },
                    { "c7aeb38a-39b2-4989-b629-6bfaf9a918d7", null, "Admin", "ADMIN" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppMessage_AppUserId1",
                table: "AppMessage",
                column: "AppUserId1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppMessage");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "27cea1a7-0dac-4e78-af96-a9d219fe2223");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "73a3657b-bc9d-4c64-be7b-3a827750a25d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c7aeb38a-39b2-4989-b629-6bfaf9a918d7");

            migrationBuilder.DropColumn(
                name: "InstructorNumber",
                table: "Instructors");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1c3fcabb-69f9-41ff-9fec-d99f551e53ab", null, "Instructor", "INSTRUCTOR" },
                    { "7a1db28c-9966-41f2-a28e-eac4165c32bd", null, "Admin", "ADMIN" },
                    { "a9942bc2-9371-4fb5-86f4-9ec553103261", null, "Student", "STUDENT" }
                });
        }
    }
}
