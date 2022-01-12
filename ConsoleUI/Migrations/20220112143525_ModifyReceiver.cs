using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConsoleUI.Migrations
{
    public partial class ModifyReceiver : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ChatId",
                table: "Receivers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChatId",
                table: "Receivers");
        }
    }
}
