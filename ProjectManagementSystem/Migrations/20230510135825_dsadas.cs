using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectManagementSystem.Migrations
{
    public partial class dsadas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "Notices",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notices_ProjectId",
                table: "Notices",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notices_Projects_ProjectId",
                table: "Notices",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notices_Projects_ProjectId",
                table: "Notices");

            migrationBuilder.DropIndex(
                name: "IX_Notices_ProjectId",
                table: "Notices");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "Notices");
        }
    }
}
