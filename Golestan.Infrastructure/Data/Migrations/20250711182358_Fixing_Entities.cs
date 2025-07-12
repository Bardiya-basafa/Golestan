using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Golestan.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class Fixing_Entities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "Terms");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "Terms");

            migrationBuilder.RenameColumn(
                name: "TermNumber",
                table: "Terms",
                newName: "TermIdentifier");

            migrationBuilder.RenameColumn(
                name: "IsFirstTerm",
                table: "Terms",
                newName: "IsClosed");

            migrationBuilder.RenameColumn(
                name: "Major",
                table: "Faculties",
                newName: "MajorName");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Courses",
                newName: "CourseName");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.RenameColumn(
                name: "TermIdentifier",
                table: "Terms",
                newName: "TermNumber");

            migrationBuilder.RenameColumn(
                name: "IsClosed",
                table: "Terms",
                newName: "IsFirstTerm");

            migrationBuilder.RenameColumn(
                name: "MajorName",
                table: "Faculties",
                newName: "Major");

            migrationBuilder.RenameColumn(
                name: "CourseName",
                table: "Courses",
                newName: "Name");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndTime",
                table: "Terms",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "StartTime",
                table: "Terms",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

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
    }
}
