using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorUi.Migrations
{
    public partial class Strddlesaddopionstrtegis : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LongStraddle_Container_ContainerId",
                table: "LongStraddle");

            migrationBuilder.DropTable(
                name: "Container");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LongStraddle",
                table: "LongStraddle");

            migrationBuilder.DropIndex(
                name: "IX_LongStraddle_ContainerId",
                table: "LongStraddle");

            migrationBuilder.DropColumn(
                name: "ContainerId",
                table: "LongStraddle");

            migrationBuilder.DropColumn(
                name: "LongStraddleLogic",
                table: "LongStraddle");

            migrationBuilder.RenameTable(
                name: "LongStraddle",
                newName: "Straddles");

            migrationBuilder.AddColumn<int>(
                name: "DbFutureId",
                table: "Straddles",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Straddles",
                table: "Straddles",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "OptionStrategies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OptionId = table.Column<int>(type: "int", nullable: false),
                    LongStraddleId = table.Column<int>(type: "int", nullable: false),
                    Volume = table.Column<int>(type: "int", nullable: false),
                    Position = table.Column<int>(type: "int", nullable: false),
                    Direction = table.Column<int>(type: "int", nullable: false),
                    StrategyLogic = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OptionStrategies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OptionStrategies_Options_OptionId",
                        column: x => x.OptionId,
                        principalTable: "Options",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OptionStrategies_Straddles_LongStraddleId",
                        column: x => x.LongStraddleId,
                        principalTable: "Straddles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Straddles_DbFutureId",
                table: "Straddles",
                column: "DbFutureId");

            migrationBuilder.CreateIndex(
                name: "IX_OptionStrategies_LongStraddleId",
                table: "OptionStrategies",
                column: "LongStraddleId");

            migrationBuilder.CreateIndex(
                name: "IX_OptionStrategies_OptionId",
                table: "OptionStrategies",
                column: "OptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Straddles_Futures_DbFutureId",
                table: "Straddles",
                column: "DbFutureId",
                principalTable: "Futures",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Straddles_Futures_DbFutureId",
                table: "Straddles");

            migrationBuilder.DropTable(
                name: "OptionStrategies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Straddles",
                table: "Straddles");

            migrationBuilder.DropIndex(
                name: "IX_Straddles_DbFutureId",
                table: "Straddles");

            migrationBuilder.DropColumn(
                name: "DbFutureId",
                table: "Straddles");

            migrationBuilder.RenameTable(
                name: "Straddles",
                newName: "LongStraddle");

            migrationBuilder.AddColumn<int>(
                name: "ContainerId",
                table: "LongStraddle",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LongStraddleLogic",
                table: "LongStraddle",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_LongStraddle",
                table: "LongStraddle",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Container",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FutureId = table.Column<int>(type: "int", nullable: false),
                    Account = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Container", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Container_Futures_FutureId",
                        column: x => x.FutureId,
                        principalTable: "Futures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LongStraddle_ContainerId",
                table: "LongStraddle",
                column: "ContainerId");

            migrationBuilder.CreateIndex(
                name: "IX_Container_FutureId",
                table: "Container",
                column: "FutureId");

            migrationBuilder.AddForeignKey(
                name: "FK_LongStraddle_Container_ContainerId",
                table: "LongStraddle",
                column: "ContainerId",
                principalTable: "Container",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
