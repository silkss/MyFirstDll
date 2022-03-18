using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorUi.Migrations
{
    public partial class ExecutedataandsenddatenowinOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ExecuteTime",
                table: "Orders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "GeneratedTime",
                table: "Orders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExecuteTime",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "GeneratedTime",
                table: "Orders");
        }
    }
}
