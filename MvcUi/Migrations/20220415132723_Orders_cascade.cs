using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MvcUi.Migrations
{
    public partial class Orders_cascade : Migration
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

            migrationBuilder.CreateTable(
                name: "Containers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FutureId = table.Column<int>(type: "int", nullable: false),
                    Account = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastTradeDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Containers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Containers_Futures_FutureId",
                        column: x => x.FutureId,
                        principalTable: "Futures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Straddles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContainerId = table.Column<int>(type: "int", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Strike = table.Column<double>(type: "float", nullable: false),
                    StraddleLogic = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Straddles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Straddles_Containers_ContainerId",
                        column: x => x.ContainerId,
                        principalTable: "Containers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OptionStrategies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OptionId = table.Column<int>(type: "int", nullable: false),
                    LongStraddleId = table.Column<int>(type: "int", nullable: false),
                    StrategyLogic = table.Column<int>(type: "int", nullable: false),
                    Volume = table.Column<int>(type: "int", nullable: false),
                    Position = table.Column<int>(type: "int", nullable: false),
                    Direction = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OptionStrategies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OptionStrategies_Options_OptionId",
                        column: x => x.OptionId,
                        principalTable: "Options",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OptionStrategies_Straddles_LongStraddleId",
                        column: x => x.LongStraddleId,
                        principalTable: "Straddles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OptionStrategyId = table.Column<int>(type: "int", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    Account = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LmtPrice = table.Column<decimal>(type: "decimal(18,10)", nullable: false),
                    Direction = table.Column<int>(type: "int", nullable: false),
                    OrderType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalQuantity = table.Column<int>(type: "int", nullable: false),
                    FilledQuantity = table.Column<int>(type: "int", nullable: false),
                    Commission = table.Column<decimal>(type: "decimal(18,10)", nullable: false),
                    AvgFilledPrice = table.Column<decimal>(type: "decimal(18,10)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GeneratedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExecuteTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_OptionStrategies_OptionStrategyId",
                        column: x => x.OptionStrategyId,
                        principalTable: "OptionStrategies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Containers_FutureId",
                table: "Containers",
                column: "FutureId");

            migrationBuilder.CreateIndex(
                name: "IX_OptionStrategies_LongStraddleId",
                table: "OptionStrategies",
                column: "LongStraddleId");

            migrationBuilder.CreateIndex(
                name: "IX_OptionStrategies_OptionId",
                table: "OptionStrategies",
                column: "OptionId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OptionStrategyId",
                table: "Orders",
                column: "OptionStrategyId");

            migrationBuilder.CreateIndex(
                name: "IX_Straddles_ContainerId",
                table: "Straddles",
                column: "ContainerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "OptionStrategies");

            migrationBuilder.DropTable(
                name: "Options");

            migrationBuilder.DropTable(
                name: "Straddles");

            migrationBuilder.DropTable(
                name: "Containers");

            migrationBuilder.DropTable(
                name: "Futures");
        }
    }
}
