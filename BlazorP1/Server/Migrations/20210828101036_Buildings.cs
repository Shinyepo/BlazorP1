using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BlazorP1.Server.Migrations
{
    public partial class Buildings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductionBuildings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BuildingName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaxLevel = table.Column<int>(type: "int", nullable: false),
                    BananaCostBase = table.Column<int>(type: "int", nullable: false),
                    BananaCostStep = table.Column<int>(type: "int", nullable: false),
                    BuildTimeBase = table.Column<int>(type: "int", nullable: false),
                    BuildTimeStep = table.Column<int>(type: "int", nullable: false),
                    BananaCountBase = table.Column<int>(type: "int", nullable: false),
                    BananaCountStep = table.Column<int>(type: "int", nullable: false),
                    TaskTimeBase = table.Column<int>(type: "int", nullable: false),
                    TaskTimeStep = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionBuildings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UnitBuildings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BuildingName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaxLevel = table.Column<int>(type: "int", nullable: false),
                    UnitLimitStep = table.Column<int>(type: "int", nullable: false),
                    UnitLimitBase = table.Column<int>(type: "int", nullable: false),
                    BananaCostBase = table.Column<int>(type: "int", nullable: false),
                    BananaCostStep = table.Column<int>(type: "int", nullable: false),
                    BuildTimeBase = table.Column<int>(type: "int", nullable: false),
                    BuildTimeStep = table.Column<int>(type: "int", nullable: false),
                    TaskTimeBase = table.Column<int>(type: "int", nullable: false),
                    TaskTimeStep = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnitBuildings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserBuildings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    BuildingId = table.Column<int>(type: "int", nullable: false),
                    BuildingLevel = table.Column<int>(type: "int", nullable: false),
                    isFinished = table.Column<bool>(type: "bit", nullable: false),
                    BuildingType = table.Column<int>(type: "int", nullable: false),
                    BuildFinishTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBuildings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UtilityBuildings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BuildingName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaxLevel = table.Column<int>(type: "int", nullable: false),
                    BananaCostBase = table.Column<int>(type: "int", nullable: false),
                    BananaCostStep = table.Column<int>(type: "int", nullable: false),
                    BuildTimeBase = table.Column<int>(type: "int", nullable: false),
                    BuildTimeStep = table.Column<int>(type: "int", nullable: false),
                    BonusType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BonusAmountBase = table.Column<int>(type: "int", nullable: false),
                    BonusAmountStep = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UtilityBuildings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserBuildingTasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserBuildingId = table.Column<int>(type: "int", nullable: false),
                    TaskEndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastTaskEndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TaskType = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBuildingTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserBuildingTasks_UserBuildings_UserBuildingId",
                        column: x => x.UserBuildingId,
                        principalTable: "UserBuildings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserBuildingTasks_UserBuildingId",
                table: "UserBuildingTasks",
                column: "UserBuildingId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductionBuildings");

            migrationBuilder.DropTable(
                name: "UnitBuildings");

            migrationBuilder.DropTable(
                name: "UserBuildingTasks");

            migrationBuilder.DropTable(
                name: "UtilityBuildings");

            migrationBuilder.DropTable(
                name: "UserBuildings");
        }
    }
}
