using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebUi.Migrations
{
    public partial class MoreabstractionsforAbstractionGod2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ask",
                table: "Futures");

            migrationBuilder.DropColumn(
                name: "Bid",
                table: "Futures");

            migrationBuilder.DropColumn(
                name: "LastPrice",
                table: "Futures");

            migrationBuilder.DropColumn(
                name: "TheorPrice",
                table: "Futures");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Ask",
                table: "Futures",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Bid",
                table: "Futures",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "LastPrice",
                table: "Futures",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TheorPrice",
                table: "Futures",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
