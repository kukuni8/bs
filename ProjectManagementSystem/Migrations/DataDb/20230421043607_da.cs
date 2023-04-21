using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectManagementSystem.Migrations.DataDb
{
    public partial class da : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Missions");

            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "Missions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Missions_StatusId",
                table: "Missions",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Missions_MissionStatuses_StatusId",
                table: "Missions",
                column: "StatusId",
                principalTable: "MissionStatuses",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Missions_MissionStatuses_StatusId",
                table: "Missions");

            migrationBuilder.DropIndex(
                name: "IX_Missions_StatusId",
                table: "Missions");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "Missions");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Missions",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
