using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectManagementSystem.Migrations
{
    public partial class ss : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MissionMissionDialogue");

            migrationBuilder.AddColumn<int>(
                name: "MissionId",
                table: "MissionDialogues",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MissionDialogues_MissionId",
                table: "MissionDialogues",
                column: "MissionId");

            migrationBuilder.AddForeignKey(
                name: "FK_MissionDialogues_Missions_MissionId",
                table: "MissionDialogues",
                column: "MissionId",
                principalTable: "Missions",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MissionDialogues_Missions_MissionId",
                table: "MissionDialogues");

            migrationBuilder.DropIndex(
                name: "IX_MissionDialogues_MissionId",
                table: "MissionDialogues");

            migrationBuilder.DropColumn(
                name: "MissionId",
                table: "MissionDialogues");

            migrationBuilder.CreateTable(
                name: "MissionMissionDialogue",
                columns: table => new
                {
                    DialoguesId = table.Column<int>(type: "int", nullable: false),
                    MissionsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MissionMissionDialogue", x => new { x.DialoguesId, x.MissionsId });
                    table.ForeignKey(
                        name: "FK_MissionMissionDialogue_MissionDialogues_DialoguesId",
                        column: x => x.DialoguesId,
                        principalTable: "MissionDialogues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MissionMissionDialogue_Missions_MissionsId",
                        column: x => x.MissionsId,
                        principalTable: "Missions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MissionMissionDialogue_MissionsId",
                table: "MissionMissionDialogue",
                column: "MissionsId");
        }
    }
}
