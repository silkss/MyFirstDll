using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebUi.Migrations
{
    public partial class Newid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FutureId",
                table: "Options",
                newName: "UnderlyingId");

            migrationBuilder.AddColumn<int>(
                name: "ConId",
                table: "Options",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ConId",
                table: "Futures",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConId",
                table: "Options");

            migrationBuilder.DropColumn(
                name: "ConId",
                table: "Futures");

            migrationBuilder.RenameColumn(
                name: "UnderlyingId",
                table: "Options",
                newName: "FutureId");
        }
    }
}
