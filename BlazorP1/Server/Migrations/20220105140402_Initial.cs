using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BlazorP1.Server.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MissionLevels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MissionName = table.Column<string>(type: "text", nullable: true),
                    Battles = table.Column<int>(type: "integer", nullable: false),
                    Wins = table.Column<int>(type: "integer", nullable: false),
                    Defeats = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MissionLevels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductionBuildings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BuildingName = table.Column<string>(type: "text", nullable: true),
                    MaxLevel = table.Column<int>(type: "integer", nullable: false),
                    BananaCostBase = table.Column<int>(type: "integer", nullable: false),
                    BananaCostStep = table.Column<int>(type: "integer", nullable: false),
                    BuildTimeBase = table.Column<int>(type: "integer", nullable: false),
                    BuildTimeStep = table.Column<int>(type: "integer", nullable: false),
                    BananaCountBase = table.Column<int>(type: "integer", nullable: false),
                    BananaCountStep = table.Column<int>(type: "integer", nullable: false),
                    TaskTimeBase = table.Column<int>(type: "integer", nullable: false),
                    TaskTimeStep = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionBuildings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Units",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BuildingId = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: true),
                    Attack = table.Column<int>(type: "integer", nullable: false),
                    Defence = table.Column<int>(type: "integer", nullable: false),
                    HitPoints = table.Column<int>(type: "integer", nullable: false),
                    BananaCost = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Units", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserBuildings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    BuildingId = table.Column<int>(type: "integer", nullable: false),
                    BuildingLevel = table.Column<int>(type: "integer", nullable: false),
                    isFinished = table.Column<bool>(type: "boolean", nullable: false),
                    BuildingType = table.Column<string>(type: "text", nullable: true),
                    BuildFinishTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    BuildStartTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBuildings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Email = table.Column<string>(type: "text", nullable: true),
                    Username = table.Column<string>(type: "text", nullable: true),
                    PasswordHash = table.Column<byte[]>(type: "bytea", nullable: true),
                    PasswordSalt = table.Column<byte[]>(type: "bytea", nullable: true),
                    Bananas = table.Column<int>(type: "integer", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    isConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    isDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    RegistrationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastLogin = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Battles = table.Column<int>(type: "integer", nullable: false),
                    Wins = table.Column<int>(type: "integer", nullable: false),
                    Defeats = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UtilityBuildings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BuildingName = table.Column<string>(type: "text", nullable: true),
                    BuildingDescription = table.Column<string>(type: "text", nullable: true),
                    MaxLevel = table.Column<int>(type: "integer", nullable: false),
                    BananaCostBase = table.Column<int>(type: "integer", nullable: false),
                    BananaCostStep = table.Column<int>(type: "integer", nullable: false),
                    BuildTimeBase = table.Column<int>(type: "integer", nullable: false),
                    BuildTimeStep = table.Column<int>(type: "integer", nullable: false),
                    BonusType = table.Column<string>(type: "text", nullable: true),
                    BonusAmountBase = table.Column<int>(type: "integer", nullable: false),
                    BonusAmountStep = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UtilityBuildings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MissionUnits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MissionLevelId = table.Column<int>(type: "integer", nullable: false),
                    UnitId = table.Column<int>(type: "integer", nullable: false),
                    HitPoints = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MissionUnits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MissionUnits_MissionLevels_MissionLevelId",
                        column: x => x.MissionLevelId,
                        principalTable: "MissionLevels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MissionUnits_Units_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Units",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UnitBuildings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BuildingName = table.Column<string>(type: "text", nullable: true),
                    UnitId = table.Column<int>(type: "integer", nullable: false),
                    MaxLevel = table.Column<int>(type: "integer", nullable: false),
                    UnitLimitStep = table.Column<int>(type: "integer", nullable: false),
                    UnitLimitBase = table.Column<int>(type: "integer", nullable: false),
                    BananaCostBase = table.Column<int>(type: "integer", nullable: false),
                    BananaCostStep = table.Column<int>(type: "integer", nullable: false),
                    BuildTimeBase = table.Column<int>(type: "integer", nullable: false),
                    BuildTimeStep = table.Column<int>(type: "integer", nullable: false),
                    TaskTimeBase = table.Column<int>(type: "integer", nullable: false),
                    TaskTimeStep = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnitBuildings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UnitBuildings_Units_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Units",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserBuildingTasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    UserBuildingId = table.Column<int>(type: "integer", nullable: false),
                    TaskEndTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastTaskEndTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TaskType = table.Column<string>(type: "text", nullable: true)
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

            migrationBuilder.CreateTable(
                name: "Battles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AttackerId = table.Column<int>(type: "integer", nullable: false),
                    OpponentId = table.Column<int>(type: "integer", nullable: false),
                    WinnerId = table.Column<int>(type: "integer", nullable: false),
                    WinnerDamage = table.Column<int>(type: "integer", nullable: false),
                    RoundsFought = table.Column<int>(type: "integer", nullable: false),
                    BattleDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Battles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Battles_Users_AttackerId",
                        column: x => x.AttackerId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Battles_Users_OpponentId",
                        column: x => x.OpponentId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Battles_Users_WinnerId",
                        column: x => x.WinnerId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserUnits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    UnitId = table.Column<int>(type: "integer", nullable: false),
                    HitPoints = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserUnits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserUnits_Units_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Units",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserUnits_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Battles_AttackerId",
                table: "Battles",
                column: "AttackerId");

            migrationBuilder.CreateIndex(
                name: "IX_Battles_OpponentId",
                table: "Battles",
                column: "OpponentId");

            migrationBuilder.CreateIndex(
                name: "IX_Battles_WinnerId",
                table: "Battles",
                column: "WinnerId");

            migrationBuilder.CreateIndex(
                name: "IX_MissionUnits_MissionLevelId",
                table: "MissionUnits",
                column: "MissionLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_MissionUnits_UnitId",
                table: "MissionUnits",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_UnitBuildings_UnitId",
                table: "UnitBuildings",
                column: "UnitId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserBuildingTasks_UserBuildingId",
                table: "UserBuildingTasks",
                column: "UserBuildingId");

            migrationBuilder.CreateIndex(
                name: "IX_UserUnits_UnitId",
                table: "UserUnits",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_UserUnits_UserId",
                table: "UserUnits",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Battles");

            migrationBuilder.DropTable(
                name: "MissionUnits");

            migrationBuilder.DropTable(
                name: "ProductionBuildings");

            migrationBuilder.DropTable(
                name: "UnitBuildings");

            migrationBuilder.DropTable(
                name: "UserBuildingTasks");

            migrationBuilder.DropTable(
                name: "UserUnits");

            migrationBuilder.DropTable(
                name: "UtilityBuildings");

            migrationBuilder.DropTable(
                name: "MissionLevels");

            migrationBuilder.DropTable(
                name: "UserBuildings");

            migrationBuilder.DropTable(
                name: "Units");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
