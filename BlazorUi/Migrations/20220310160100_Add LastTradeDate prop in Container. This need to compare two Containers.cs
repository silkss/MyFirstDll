using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorUi.Migrations
{
    public partial class AddLastTradeDatepropinContainerThisneedtocomparetwoContainers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Containers_Futures_FutureId",
                table: "Containers");

            migrationBuilder.AlterColumn<int>(
                name: "FutureId",
                table: "Containers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastTradeDate",
                table: "Containers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_Containers_Futures_FutureId",
                table: "Containers",
                column: "FutureId",
                principalTable: "Futures",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Containers_Futures_FutureId",
                table: "Containers");

            migrationBuilder.DropColumn(
                name: "LastTradeDate",
                table: "Containers");

            migrationBuilder.AlterColumn<int>(
                name: "FutureId",
                table: "Containers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Containers_Futures_FutureId",
                table: "Containers",
                column: "FutureId",
                principalTable: "Futures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
