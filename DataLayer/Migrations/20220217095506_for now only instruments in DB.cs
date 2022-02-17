using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLayer.Migrations
{
    public partial class fornowonlyinstrumentsinDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DbOrders");

            migrationBuilder.DropTable(
                name: "OptionStrategies");

            migrationBuilder.DropTable(
                name: "LongStraddles");

            migrationBuilder.DropTable(
                name: "MainStrategies");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Option",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Future",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Option");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Future");

            migrationBuilder.CreateTable(
                name: "MainStrategies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MainStrategies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LongStraddles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ContainerId = table.Column<int>(type: "INTEGER", nullable: false),
                    LongStraddleLogic = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LongStraddles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LongStraddles_MainStrategies_ContainerId",
                        column: x => x.ContainerId,
                        principalTable: "MainStrategies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OptionStrategies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LongStraddleId = table.Column<int>(type: "INTEGER", nullable: false),
                    OptionId = table.Column<int>(type: "INTEGER", nullable: false),
                    Direction = table.Column<int>(type: "INTEGER", nullable: false),
                    InstrumentId = table.Column<int>(type: "INTEGER", nullable: false),
                    StrategyLogic = table.Column<int>(type: "INTEGER", nullable: false),
                    Volume = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OptionStrategies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OptionStrategies_LongStraddles_LongStraddleId",
                        column: x => x.LongStraddleId,
                        principalTable: "LongStraddles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OptionStrategies_Option_OptionId",
                        column: x => x.OptionId,
                        principalTable: "Option",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DbOrders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OptionStrategyId = table.Column<int>(type: "INTEGER", nullable: false),
                    Account = table.Column<string>(type: "TEXT", nullable: true),
                    AvgFilledPrice = table.Column<decimal>(type: "TEXT", nullable: false),
                    Commission = table.Column<decimal>(type: "TEXT", nullable: false),
                    Direction = table.Column<int>(type: "INTEGER", nullable: false),
                    FilledQuantity = table.Column<int>(type: "INTEGER", nullable: false),
                    LmtPrice = table.Column<decimal>(type: "TEXT", nullable: false),
                    OrderType = table.Column<string>(type: "TEXT", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: true),
                    TotalQuantity = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DbOrders_OptionStrategies_OptionStrategyId",
                        column: x => x.OptionStrategyId,
                        principalTable: "OptionStrategies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DbOrders_OptionStrategyId",
                table: "DbOrders",
                column: "OptionStrategyId");

            migrationBuilder.CreateIndex(
                name: "IX_LongStraddles_ContainerId",
                table: "LongStraddles",
                column: "ContainerId");

            migrationBuilder.CreateIndex(
                name: "IX_OptionStrategies_LongStraddleId",
                table: "OptionStrategies",
                column: "LongStraddleId");

            migrationBuilder.CreateIndex(
                name: "IX_OptionStrategies_OptionId",
                table: "OptionStrategies",
                column: "OptionId");
        }
    }
}
