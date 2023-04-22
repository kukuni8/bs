using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectManagementSystem.Migrations
{
    public partial class dfasda : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Missions_MissionPriority_PriorityId",
                table: "Missions");

            migrationBuilder.DropForeignKey(
                name: "FK_Missions_MissionStatuses_StatusId",
                table: "Missions");

            migrationBuilder.DropForeignKey(
                name: "FK_Missions_Projects_ProjectId",
                table: "Missions");

            migrationBuilder.AlterColumn<int>(
                name: "StatusId",
                table: "Missions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ProjectId",
                table: "Missions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PriorityId",
                table: "Missions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Missions_MissionPriority_PriorityId",
                table: "Missions",
                column: "PriorityId",
                principalTable: "MissionPriority",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Missions_MissionStatuses_StatusId",
                table: "Missions",
                column: "StatusId",
                principalTable: "MissionStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Missions_Projects_ProjectId",
                table: "Missions",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Missions_MissionPriority_PriorityId",
                table: "Missions");

            migrationBuilder.DropForeignKey(
                name: "FK_Missions_MissionStatuses_StatusId",
                table: "Missions");

            migrationBuilder.DropForeignKey(
                name: "FK_Missions_Projects_ProjectId",
                table: "Missions");

            migrationBuilder.AlterColumn<int>(
                name: "StatusId",
                table: "Missions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ProjectId",
                table: "Missions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "PriorityId",
                table: "Missions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Missions_MissionPriority_PriorityId",
                table: "Missions",
                column: "PriorityId",
                principalTable: "MissionPriority",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Missions_MissionStatuses_StatusId",
                table: "Missions",
                column: "StatusId",
                principalTable: "MissionStatuses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Missions_Projects_ProjectId",
                table: "Missions",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id");
        }
    }
}
