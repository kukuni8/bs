using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectManagementSystem.Migrations.DataDb
{
    public partial class ii : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Level",
                table: "Risks");

            migrationBuilder.DropColumn(
                name: "RiskType",
                table: "Risks");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Risks");

            migrationBuilder.DropColumn(
                name: "Functionary",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "PutForward",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "Executor",
                table: "Missions");

            migrationBuilder.DropColumn(
                name: "DefectStatus",
                table: "Defects");

            migrationBuilder.DropColumn(
                name: "DefectType",
                table: "Defects");

            migrationBuilder.AddColumn<int>(
                name: "LevelId",
                table: "Risks",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RiskTypeId",
                table: "Risks",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "Risks",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FunctionaryId",
                table: "Projects",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PutForwardId",
                table: "Projects",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "Projects",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ExecutorId",
                table: "Missions",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "Missions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FunctionaryId",
                table: "Defects",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PutForwardId",
                table: "Defects",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "Defects",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TypeId",
                table: "Defects",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ApplicationUser",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IdCardNo = table.Column<string>(type: "nvarchar(18)", maxLength: 18, nullable: true),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUser", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Dialogues",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MissionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dialogues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Dialogues_ApplicationUser_UserId",
                        column: x => x.UserId,
                        principalTable: "ApplicationUser",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Dialogues_Missions_MissionId",
                        column: x => x.MissionId,
                        principalTable: "Missions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Risks_LevelId",
                table: "Risks",
                column: "LevelId");

            migrationBuilder.CreateIndex(
                name: "IX_Risks_RiskTypeId",
                table: "Risks",
                column: "RiskTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Risks_StatusId",
                table: "Risks",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_FunctionaryId",
                table: "Projects",
                column: "FunctionaryId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_PutForwardId",
                table: "Projects",
                column: "PutForwardId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_StatusId",
                table: "Projects",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Missions_ExecutorId",
                table: "Missions",
                column: "ExecutorId");

            migrationBuilder.CreateIndex(
                name: "IX_Missions_ProjectId",
                table: "Missions",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Defects_FunctionaryId",
                table: "Defects",
                column: "FunctionaryId");

            migrationBuilder.CreateIndex(
                name: "IX_Defects_PutForwardId",
                table: "Defects",
                column: "PutForwardId");

            migrationBuilder.CreateIndex(
                name: "IX_Defects_StatusId",
                table: "Defects",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Defects_TypeId",
                table: "Defects",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Dialogues_MissionId",
                table: "Dialogues",
                column: "MissionId");

            migrationBuilder.CreateIndex(
                name: "IX_Dialogues_UserId",
                table: "Dialogues",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Defects_ApplicationUser_FunctionaryId",
                table: "Defects",
                column: "FunctionaryId",
                principalTable: "ApplicationUser",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Defects_ApplicationUser_PutForwardId",
                table: "Defects",
                column: "PutForwardId",
                principalTable: "ApplicationUser",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Defects_DefectStatuses_StatusId",
                table: "Defects",
                column: "StatusId",
                principalTable: "DefectStatuses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Defects_DefectTypes_TypeId",
                table: "Defects",
                column: "TypeId",
                principalTable: "DefectTypes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Missions_ApplicationUser_ExecutorId",
                table: "Missions",
                column: "ExecutorId",
                principalTable: "ApplicationUser",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Missions_Projects_ProjectId",
                table: "Missions",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_ApplicationUser_FunctionaryId",
                table: "Projects",
                column: "FunctionaryId",
                principalTable: "ApplicationUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_ApplicationUser_PutForwardId",
                table: "Projects",
                column: "PutForwardId",
                principalTable: "ApplicationUser",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_ProjectStatuses_StatusId",
                table: "Projects",
                column: "StatusId",
                principalTable: "ProjectStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Risks_RiskLevels_LevelId",
                table: "Risks",
                column: "LevelId",
                principalTable: "RiskLevels",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Risks_RiskStatuses_StatusId",
                table: "Risks",
                column: "StatusId",
                principalTable: "RiskStatuses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Risks_RiskTypes_RiskTypeId",
                table: "Risks",
                column: "RiskTypeId",
                principalTable: "RiskTypes",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Defects_ApplicationUser_FunctionaryId",
                table: "Defects");

            migrationBuilder.DropForeignKey(
                name: "FK_Defects_ApplicationUser_PutForwardId",
                table: "Defects");

            migrationBuilder.DropForeignKey(
                name: "FK_Defects_DefectStatuses_StatusId",
                table: "Defects");

            migrationBuilder.DropForeignKey(
                name: "FK_Defects_DefectTypes_TypeId",
                table: "Defects");

            migrationBuilder.DropForeignKey(
                name: "FK_Missions_ApplicationUser_ExecutorId",
                table: "Missions");

            migrationBuilder.DropForeignKey(
                name: "FK_Missions_Projects_ProjectId",
                table: "Missions");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_ApplicationUser_FunctionaryId",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_ApplicationUser_PutForwardId",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_ProjectStatuses_StatusId",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_Risks_RiskLevels_LevelId",
                table: "Risks");

            migrationBuilder.DropForeignKey(
                name: "FK_Risks_RiskStatuses_StatusId",
                table: "Risks");

            migrationBuilder.DropForeignKey(
                name: "FK_Risks_RiskTypes_RiskTypeId",
                table: "Risks");

            migrationBuilder.DropTable(
                name: "Dialogues");

            migrationBuilder.DropTable(
                name: "ApplicationUser");

            migrationBuilder.DropIndex(
                name: "IX_Risks_LevelId",
                table: "Risks");

            migrationBuilder.DropIndex(
                name: "IX_Risks_RiskTypeId",
                table: "Risks");

            migrationBuilder.DropIndex(
                name: "IX_Risks_StatusId",
                table: "Risks");

            migrationBuilder.DropIndex(
                name: "IX_Projects_FunctionaryId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_PutForwardId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_StatusId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Missions_ExecutorId",
                table: "Missions");

            migrationBuilder.DropIndex(
                name: "IX_Missions_ProjectId",
                table: "Missions");

            migrationBuilder.DropIndex(
                name: "IX_Defects_FunctionaryId",
                table: "Defects");

            migrationBuilder.DropIndex(
                name: "IX_Defects_PutForwardId",
                table: "Defects");

            migrationBuilder.DropIndex(
                name: "IX_Defects_StatusId",
                table: "Defects");

            migrationBuilder.DropIndex(
                name: "IX_Defects_TypeId",
                table: "Defects");

            migrationBuilder.DropColumn(
                name: "LevelId",
                table: "Risks");

            migrationBuilder.DropColumn(
                name: "RiskTypeId",
                table: "Risks");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "Risks");

            migrationBuilder.DropColumn(
                name: "FunctionaryId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "PutForwardId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ExecutorId",
                table: "Missions");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "Missions");

            migrationBuilder.DropColumn(
                name: "FunctionaryId",
                table: "Defects");

            migrationBuilder.DropColumn(
                name: "PutForwardId",
                table: "Defects");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "Defects");

            migrationBuilder.DropColumn(
                name: "TypeId",
                table: "Defects");

            migrationBuilder.AddColumn<string>(
                name: "Level",
                table: "Risks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RiskType",
                table: "Risks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Risks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Functionary",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PutForward",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Executor",
                table: "Missions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DefectStatus",
                table: "Defects",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DefectType",
                table: "Defects",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
