using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Golestan.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Fix_Intructor_Relation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "CourseInstructors",
                columns: table => new
                {
                    CoursesId = table.Column<int>(type: "int", nullable: false),
                    InstructorsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseInstructors", x => new { x.CoursesId, x.InstructorsId });
                    table.ForeignKey(
                        name: "FK_CourseInstructors_Courses_CoursesId",
                        column: x => x.CoursesId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseInstructors_Instructors_InstructorsId",
                        column: x => x.InstructorsId,
                        principalTable: "Instructors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "14c9dcae-da6a-4833-af53-7dbda657612b", null, "Instructor", "INSTRUCTOR" },
                    { "773b09da-08f1-425a-a3a1-bae5ea00aad8", null, "Student", "STUDENT" },
                    { "d3e9e74d-5d7d-4ed5-bfe3-e2310181fe68", null, "Admin", "ADMIN" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseInstructors_InstructorsId",
                table: "CourseInstructors",
                column: "InstructorsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseInstructors");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "14c9dcae-da6a-4833-af53-7dbda657612b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "773b09da-08f1-425a-a3a1-bae5ea00aad8");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d3e9e74d-5d7d-4ed5-bfe3-e2310181fe68");

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
    }
}
