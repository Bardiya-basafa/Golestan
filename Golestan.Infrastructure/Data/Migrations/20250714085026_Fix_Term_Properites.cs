using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Golestan.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class Fix_Term_Properites : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.AddColumn<string>(
                name: "TermName",
                table: "Terms",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "5adf2454-fd08-407c-bb5a-e01fca14ce90", null, "Instructor", "INSTRUCTOR" },
                    { "89ee0464-092c-4b36-b965-61e2df23e16c", null, "Admin", "ADMIN" },
                    { "911cf426-a988-4815-a7fd-bb3800db3e6d", null, "Student", "STUDENT" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5adf2454-fd08-407c-bb5a-e01fca14ce90");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "89ee0464-092c-4b36-b965-61e2df23e16c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "911cf426-a988-4815-a7fd-bb3800db3e6d");

            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "Terms");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "Terms");

            migrationBuilder.DropColumn(
                name: "TermName",
                table: "Terms");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "27cea1a7-0dac-4e78-af96-a9d219fe2223", null, "Student", "STUDENT" },
                    { "73a3657b-bc9d-4c64-be7b-3a827750a25d", null, "Instructor", "INSTRUCTOR" },
                    { "c7aeb38a-39b2-4989-b629-6bfaf9a918d7", null, "Admin", "ADMIN" }
                });
        }
    }
}
