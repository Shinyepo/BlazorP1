using Microsoft.EntityFrameworkCore.Migrations;

namespace BlazorP1.Server.Migrations
{
    public partial class MissionLevelId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MissionUnits_MissionLevels_MissionLevelId",
                table: "MissionUnits");

            migrationBuilder.DropColumn(
                name: "MissionId",
                table: "MissionUnits");

            migrationBuilder.AlterColumn<int>(
                name: "MissionLevelId",
                table: "MissionUnits",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MissionUnits_MissionLevels_MissionLevelId",
                table: "MissionUnits",
                column: "MissionLevelId",
                principalTable: "MissionLevels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MissionUnits_MissionLevels_MissionLevelId",
                table: "MissionUnits");

            migrationBuilder.AlterColumn<int>(
                name: "MissionLevelId",
                table: "MissionUnits",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "MissionId",
                table: "MissionUnits",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_MissionUnits_MissionLevels_MissionLevelId",
                table: "MissionUnits",
                column: "MissionLevelId",
                principalTable: "MissionLevels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
