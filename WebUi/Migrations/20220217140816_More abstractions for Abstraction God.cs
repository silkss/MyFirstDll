using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebUi.Migrations
{
    public partial class MoreabstractionsforAbstractionGod : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Option_Future_FutureId",
                table: "Option");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Option",
                table: "Option");

            migrationBuilder.DropIndex(
                name: "IX_Option_FutureId",
                table: "Option");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Future",
                table: "Future");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Option");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Future");

            migrationBuilder.RenameTable(
                name: "Option",
                newName: "Options");

            migrationBuilder.RenameTable(
                name: "Future",
                newName: "Futures");

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

            migrationBuilder.AddPrimaryKey(
                name: "PK_Options",
                table: "Options",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Futures",
                table: "Futures",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Options",
                table: "Options");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Futures",
                table: "Futures");

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

            migrationBuilder.RenameTable(
                name: "Options",
                newName: "Option");

            migrationBuilder.RenameTable(
                name: "Futures",
                newName: "Future");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Option",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Future",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Option",
                table: "Option",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Future",
                table: "Future",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Option_FutureId",
                table: "Option",
                column: "FutureId");

            migrationBuilder.AddForeignKey(
                name: "FK_Option_Future_FutureId",
                table: "Option",
                column: "FutureId",
                principalTable: "Future",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
