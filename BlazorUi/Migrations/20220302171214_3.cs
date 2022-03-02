using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorUi.Migrations
{
    public partial class _3 : Migration
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
                name: "Containers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FutureId = table.Column<int>(type: "int", nullable: true),
                    Account = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Containers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Containers_Futures_FutureId",
                        column: x => x.FutureId,
                        principalTable: "Futures",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DbOption",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FutureId = table.Column<int>(type: "int", nullable: true),
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
                    table.PrimaryKey("PK_DbOption", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DbOption_Futures_FutureId",
                        column: x => x.FutureId,
                        principalTable: "Futures",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Straddles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContainerId = table.Column<int>(type: "int", nullable: true),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Strike = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Straddles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Straddles_Containers_ContainerId",
                        column: x => x.ContainerId,
                        principalTable: "Containers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OptionStrategy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OptionId = table.Column<int>(type: "int", nullable: true),
                    LongStraddleId = table.Column<int>(type: "int", nullable: true),
                    Volume = table.Column<int>(type: "int", nullable: false),
                    Position = table.Column<int>(type: "int", nullable: false),
                    Direction = table.Column<int>(type: "int", nullable: false),
                    StrategyLogic = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OptionStrategy", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OptionStrategy_DbOption_OptionId",
                        column: x => x.OptionId,
                        principalTable: "DbOption",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OptionStrategy_Straddles_LongStraddleId",
                        column: x => x.LongStraddleId,
                        principalTable: "Straddles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Containers_FutureId",
                table: "Containers",
                column: "FutureId");

            migrationBuilder.CreateIndex(
                name: "IX_DbOption_FutureId",
                table: "DbOption",
                column: "FutureId");

            migrationBuilder.CreateIndex(
                name: "IX_OptionStrategy_LongStraddleId",
                table: "OptionStrategy",
                column: "LongStraddleId");

            migrationBuilder.CreateIndex(
                name: "IX_OptionStrategy_OptionId",
                table: "OptionStrategy",
                column: "OptionId");

            migrationBuilder.CreateIndex(
                name: "IX_Straddles_ContainerId",
                table: "Straddles",
                column: "ContainerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OptionStrategy");

            migrationBuilder.DropTable(
                name: "DbOption");

            migrationBuilder.DropTable(
                name: "Straddles");

            migrationBuilder.DropTable(
                name: "Containers");

            migrationBuilder.DropTable(
                name: "Futures");
        }
    }
}
