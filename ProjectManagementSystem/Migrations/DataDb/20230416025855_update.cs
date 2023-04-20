using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectManagementSystem.Migrations.DataDb
{
    public partial class update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUser_Projects_ProjectId",
                table: "ApplicationUser");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_ProjectStatuses_StatusId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_ApplicationUser_ProjectId",
                table: "ApplicationUser");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "ApplicationUser");

            migrationBuilder.AlterColumn<int>(
                name: "StatusId",
                table: "Projects",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PutForward",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SelectFunctionary",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_ProjectStatuses_StatusId",
                table: "Projects",
                column: "StatusId",
                principalTable: "ProjectStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_ProjectStatuses_StatusId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "PutForward",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "SelectFunctionary",
                table: "Projects");

            migrationBuilder.AlterColumn<int>(
                name: "StatusId",
                table: "Projects",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "ApplicationUser",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUser_ProjectId",
                table: "ApplicationUser",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUser_Projects_ProjectId",
                table: "ApplicationUser",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_ProjectStatuses_StatusId",
                table: "Projects",
                column: "StatusId",
                principalTable: "ProjectStatuses",
                principalColumn: "Id");
        }
    }
}
