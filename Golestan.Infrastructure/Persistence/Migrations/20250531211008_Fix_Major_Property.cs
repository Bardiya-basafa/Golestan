using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Golestan.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Fix_Major_Property : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6abf27ba-3909-486c-abed-5a50d64aaa6d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "df378605-0833-41d5-92f2-a153b3b1330f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ee0e37d2-a4b1-4582-82d2-d6fa1b72fcee");

            migrationBuilder.DropColumn(
                name: "Major",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Major",
                table: "Instructors");

            migrationBuilder.DropColumn(
                name: "Major",
                table: "Faculties");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Faculties",
                newName: "MajorName");

            migrationBuilder.AddColumn<string>(
                name: "BuildingName",
                table: "Faculties",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "Faculties",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "BuildingName",
                table: "Faculties");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Faculties");

            migrationBuilder.RenameColumn(
                name: "MajorName",
                table: "Faculties",
                newName: "Name");

            migrationBuilder.AddColumn<int>(
                name: "Major",
                table: "Students",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Major",
                table: "Instructors",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Major",
                table: "Faculties",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "6abf27ba-3909-486c-abed-5a50d64aaa6d", null, "Instructor", "INSTRUCTOR" },
                    { "df378605-0833-41d5-92f2-a153b3b1330f", null, "Admin", "ADMIN" },
                    { "ee0e37d2-a4b1-4582-82d2-d6fa1b72fcee", null, "Student", "STUDENT" }
                });
        }
    }
}
