using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorUi.Migrations
{
    public partial class AddlinkbetweenFutureandOptions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FutureId",
                table: "Options",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Options_FutureId",
                table: "Options",
                column: "FutureId");

            migrationBuilder.AddForeignKey(
                name: "FK_Options_Futures_FutureId",
                table: "Options",
                column: "FutureId",
                principalTable: "Futures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Options_Futures_FutureId",
                table: "Options");

            migrationBuilder.DropIndex(
                name: "IX_Options_FutureId",
                table: "Options");

            migrationBuilder.DropColumn(
                name: "FutureId",
                table: "Options");
        }
    }
}
