using Microsoft.EntityFrameworkCore.Migrations;

namespace BlazorP1.Server.Migrations
{
    public partial class UnitIdToBuilding : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BuildingId",
                table: "Units",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UnitId",
                table: "UnitBuildings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UnitBuildings_UnitId",
                table: "UnitBuildings",
                column: "UnitId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UnitBuildings_Units_UnitId",
                table: "UnitBuildings",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UnitBuildings_Units_UnitId",
                table: "UnitBuildings");

            migrationBuilder.DropIndex(
                name: "IX_UnitBuildings_UnitId",
                table: "UnitBuildings");

            migrationBuilder.DropColumn(
                name: "BuildingId",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "UnitId",
                table: "UnitBuildings");
        }
    }
}
