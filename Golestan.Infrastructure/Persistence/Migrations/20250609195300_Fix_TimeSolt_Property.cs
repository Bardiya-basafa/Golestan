using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Golestan.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Fix_TimeSolt_Property : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TimeSlots");

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

            migrationBuilder.AddColumn<int>(
                name: "DayOfWeek",
                table: "Sections",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TimeSlot",
                table: "Sections",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "4c123eae-2aa4-48b4-8d94-1223e8961114", null, "Admin", "ADMIN" },
                    { "f3644f19-0b96-4dfc-bc14-f52bb7710260", null, "Student", "STUDENT" },
                    { "f6796af3-8125-4fbf-919a-f0ba1a841f5e", null, "Instructor", "INSTRUCTOR" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4c123eae-2aa4-48b4-8d94-1223e8961114");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f3644f19-0b96-4dfc-bc14-f52bb7710260");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f6796af3-8125-4fbf-919a-f0ba1a841f5e");

            migrationBuilder.DropColumn(
                name: "DayOfWeek",
                table: "Sections");

            migrationBuilder.DropColumn(
                name: "TimeSlot",
                table: "Sections");

            migrationBuilder.CreateTable(
                name: "TimeSlots",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SectionId = table.Column<int>(type: "int", nullable: false),
                    ClassroomId = table.Column<int>(type: "int", nullable: true),
                    DayOfWeek = table.Column<int>(type: "int", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeSlots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TimeSlots_Classrooms_ClassroomId",
                        column: x => x.ClassroomId,
                        principalTable: "Classrooms",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TimeSlots_Sections_SectionId",
                        column: x => x.SectionId,
                        principalTable: "Sections",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0c8b7909-9ba4-4333-aa8c-1bc3a1d353bf", null, "Instructor", "INSTRUCTOR" },
                    { "c0c11ac0-41f0-4628-8818-efa9a1dfa286", null, "Admin", "ADMIN" },
                    { "f3ffcd36-1ca0-4ebb-a71e-786f665b9e1e", null, "Student", "STUDENT" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_TimeSlots_ClassroomId",
                table: "TimeSlots",
                column: "ClassroomId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeSlots_SectionId",
                table: "TimeSlots",
                column: "SectionId",
                unique: true);
        }
    }
}
