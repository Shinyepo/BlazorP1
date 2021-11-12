using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BlazorP1.Server.Migrations
{
    public partial class BuildStartTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "BuildStartTime",
                table: "UserBuildings",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BuildStartTime",
                table: "UserBuildings");
        }
    }
}
