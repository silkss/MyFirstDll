using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorUi.Migrations
{
    public partial class RmoveContainesfromDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Containers_Futures_FutureId",
                table: "Containers");

            migrationBuilder.DropForeignKey(
                name: "FK_LongStraddle_Containers_ContainerId",
                table: "LongStraddle");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Containers",
                table: "Containers");

            migrationBuilder.RenameTable(
                name: "Containers",
                newName: "Container");

            migrationBuilder.RenameIndex(
                name: "IX_Containers_FutureId",
                table: "Container",
                newName: "IX_Container_FutureId");

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpirationDate",
                table: "LongStraddle",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<double>(
                name: "Strike",
                table: "LongStraddle",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Container",
                table: "Container",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Container_Futures_FutureId",
                table: "Container",
                column: "FutureId",
                principalTable: "Futures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LongStraddle_Container_ContainerId",
                table: "LongStraddle",
                column: "ContainerId",
                principalTable: "Container",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Container_Futures_FutureId",
                table: "Container");

            migrationBuilder.DropForeignKey(
                name: "FK_LongStraddle_Container_ContainerId",
                table: "LongStraddle");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Container",
                table: "Container");

            migrationBuilder.DropColumn(
                name: "ExpirationDate",
                table: "LongStraddle");

            migrationBuilder.DropColumn(
                name: "Strike",
                table: "LongStraddle");

            migrationBuilder.RenameTable(
                name: "Container",
                newName: "Containers");

            migrationBuilder.RenameIndex(
                name: "IX_Container_FutureId",
                table: "Containers",
                newName: "IX_Containers_FutureId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Containers",
                table: "Containers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Containers_Futures_FutureId",
                table: "Containers",
                column: "FutureId",
                principalTable: "Futures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LongStraddle_Containers_ContainerId",
                table: "LongStraddle",
                column: "ContainerId",
                principalTable: "Containers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
