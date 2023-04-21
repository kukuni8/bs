using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectManagementSystem.Migrations.DataDb
{
    public partial class Initssssss : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Missions");

            migrationBuilder.AddColumn<int>(
                name: "TypeId",
                table: "Missions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MissionTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MissionTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Missions_TypeId",
                table: "Missions",
                column: "TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Missions_MissionTypes_TypeId",
                table: "Missions",
                column: "TypeId",
                principalTable: "MissionTypes",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Missions_MissionTypes_TypeId",
                table: "Missions");

            migrationBuilder.DropTable(
                name: "MissionTypes");

            migrationBuilder.DropIndex(
                name: "IX_Missions_TypeId",
                table: "Missions");

            migrationBuilder.DropColumn(
                name: "TypeId",
                table: "Missions");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Missions",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
