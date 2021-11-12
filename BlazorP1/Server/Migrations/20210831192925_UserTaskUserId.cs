using Microsoft.EntityFrameworkCore.Migrations;

namespace BlazorP1.Server.Migrations
{
    public partial class UserTaskUserId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "UserBuildingTasks",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UserBuildingTasks");
        }
    }
}
