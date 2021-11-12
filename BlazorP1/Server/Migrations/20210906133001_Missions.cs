using Microsoft.EntityFrameworkCore.Migrations;

namespace BlazorP1.Server.Migrations
{
    public partial class Missions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MissionLevels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MissionName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Battles = table.Column<int>(type: "int", nullable: false),
                    Wins = table.Column<int>(type: "int", nullable: false),
                    Defeats = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MissionLevels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MissionUnits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MissionId = table.Column<int>(type: "int", nullable: false),
                    UnitId = table.Column<int>(type: "int", nullable: false),
                    HitPoints = table.Column<int>(type: "int", nullable: false),
                    MissionLevelId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MissionUnits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MissionUnits_MissionLevels_MissionLevelId",
                        column: x => x.MissionLevelId,
                        principalTable: "MissionLevels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MissionUnits_Units_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Units",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MissionUnits_MissionLevelId",
                table: "MissionUnits",
                column: "MissionLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_MissionUnits_UnitId",
                table: "MissionUnits",
                column: "UnitId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MissionUnits");

            migrationBuilder.DropTable(
                name: "MissionLevels");
        }
    }
}
