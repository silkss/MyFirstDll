using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorUi.Migrations
{
    public partial class Createdcontainrfortrading : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Futures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConId = table.Column<int>(type: "int", nullable: false),
                    LocalSymbol = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Symbol = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Echange = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MinTick = table.Column<decimal>(type: "decimal(18,10)", nullable: false),
                    Multiplier = table.Column<int>(type: "int", nullable: false),
                    MarketRule = table.Column<int>(type: "int", nullable: false),
                    LastTradeDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InstumentType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Futures", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Options",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConId = table.Column<int>(type: "int", nullable: false),
                    Strike = table.Column<decimal>(type: "decimal(18,10)", nullable: false),
                    TradingClass = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OptionType = table.Column<int>(type: "int", nullable: false),
                    UnderlyingId = table.Column<int>(type: "int", nullable: false),
                    LocalSymbol = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Symbol = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Echange = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MinTick = table.Column<decimal>(type: "decimal(18,10)", nullable: false),
                    Multiplier = table.Column<int>(type: "int", nullable: false),
                    MarketRule = table.Column<int>(type: "int", nullable: false),
                    LastTradeDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InstumentType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Options", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Futures");

            migrationBuilder.DropTable(
                name: "Options");
        }
    }
}
