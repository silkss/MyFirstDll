using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorUi.Migrations
{
    public partial class Addedorderstable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DbOption_Futures_FutureId",
                table: "DbOption");

            migrationBuilder.DropForeignKey(
                name: "FK_OptionStrategy_DbOption_OptionId",
                table: "OptionStrategy");

            migrationBuilder.DropForeignKey(
                name: "FK_OptionStrategy_Straddles_LongStraddleId",
                table: "OptionStrategy");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OptionStrategy",
                table: "OptionStrategy");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DbOption",
                table: "DbOption");

            migrationBuilder.RenameTable(
                name: "OptionStrategy",
                newName: "OptionStrategies");

            migrationBuilder.RenameTable(
                name: "DbOption",
                newName: "Options");

            migrationBuilder.RenameIndex(
                name: "IX_OptionStrategy_OptionId",
                table: "OptionStrategies",
                newName: "IX_OptionStrategies_OptionId");

            migrationBuilder.RenameIndex(
                name: "IX_OptionStrategy_LongStraddleId",
                table: "OptionStrategies",
                newName: "IX_OptionStrategies_LongStraddleId");

            migrationBuilder.RenameIndex(
                name: "IX_DbOption_FutureId",
                table: "Options",
                newName: "IX_Options_FutureId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OptionStrategies",
                table: "OptionStrategies",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Options",
                table: "Options",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OptionStrategyId = table.Column<int>(type: "int", nullable: true),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    Account = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LmtPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Direction = table.Column<int>(type: "int", nullable: false),
                    OrderType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalQuantity = table.Column<int>(type: "int", nullable: false),
                    FilledQuantity = table.Column<int>(type: "int", nullable: false),
                    Commission = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AvgFilledPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_OptionStrategies_OptionStrategyId",
                        column: x => x.OptionStrategyId,
                        principalTable: "OptionStrategies",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OptionStrategyId",
                table: "Orders",
                column: "OptionStrategyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Options_Futures_FutureId",
                table: "Options",
                column: "FutureId",
                principalTable: "Futures",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OptionStrategies_Options_OptionId",
                table: "OptionStrategies",
                column: "OptionId",
                principalTable: "Options",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OptionStrategies_Straddles_LongStraddleId",
                table: "OptionStrategies",
                column: "LongStraddleId",
                principalTable: "Straddles",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Options_Futures_FutureId",
                table: "Options");

            migrationBuilder.DropForeignKey(
                name: "FK_OptionStrategies_Options_OptionId",
                table: "OptionStrategies");

            migrationBuilder.DropForeignKey(
                name: "FK_OptionStrategies_Straddles_LongStraddleId",
                table: "OptionStrategies");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OptionStrategies",
                table: "OptionStrategies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Options",
                table: "Options");

            migrationBuilder.RenameTable(
                name: "OptionStrategies",
                newName: "OptionStrategy");

            migrationBuilder.RenameTable(
                name: "Options",
                newName: "DbOption");

            migrationBuilder.RenameIndex(
                name: "IX_OptionStrategies_OptionId",
                table: "OptionStrategy",
                newName: "IX_OptionStrategy_OptionId");

            migrationBuilder.RenameIndex(
                name: "IX_OptionStrategies_LongStraddleId",
                table: "OptionStrategy",
                newName: "IX_OptionStrategy_LongStraddleId");

            migrationBuilder.RenameIndex(
                name: "IX_Options_FutureId",
                table: "DbOption",
                newName: "IX_DbOption_FutureId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OptionStrategy",
                table: "OptionStrategy",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DbOption",
                table: "DbOption",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DbOption_Futures_FutureId",
                table: "DbOption",
                column: "FutureId",
                principalTable: "Futures",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OptionStrategy_DbOption_OptionId",
                table: "OptionStrategy",
                column: "OptionId",
                principalTable: "DbOption",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OptionStrategy_Straddles_LongStraddleId",
                table: "OptionStrategy",
                column: "LongStraddleId",
                principalTable: "Straddles",
                principalColumn: "Id");
        }
    }
}
