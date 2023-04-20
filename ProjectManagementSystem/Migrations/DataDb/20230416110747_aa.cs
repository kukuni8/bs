using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectManagementSystem.Migrations.DataDb
{
    public partial class aa : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Defects_DefectStatuses_DefectStatusId",
                table: "Defects");

            migrationBuilder.DropForeignKey(
                name: "FK_Defects_DefectTypes_DefectTypeId",
                table: "Defects");

            migrationBuilder.DropForeignKey(
                name: "FK_Risks_RiskLevels_RiskLevelId",
                table: "Risks");

            migrationBuilder.DropForeignKey(
                name: "FK_Risks_RiskStatuses_RiskStatusId",
                table: "Risks");

            migrationBuilder.DropIndex(
                name: "IX_Risks_RiskLevelId",
                table: "Risks");

            migrationBuilder.DropIndex(
                name: "IX_Risks_RiskStatusId",
                table: "Risks");

            migrationBuilder.DropIndex(
                name: "IX_Defects_DefectStatusId",
                table: "Defects");

            migrationBuilder.DropIndex(
                name: "IX_Defects_DefectTypeId",
                table: "Defects");

            migrationBuilder.DropColumn(
                name: "RiskLevelId",
                table: "Risks");

            migrationBuilder.DropColumn(
                name: "RiskStatusId",
                table: "Risks");

            migrationBuilder.DropColumn(
                name: "DefectStatusId",
                table: "Defects");

            migrationBuilder.DropColumn(
                name: "DefectTypeId",
                table: "Defects");

            migrationBuilder.RenameColumn(
                name: "RiskSolution",
                table: "Risks",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "RiskProbability",
                table: "Risks",
                newName: "Solution");

            migrationBuilder.RenameColumn(
                name: "RiskName",
                table: "Risks",
                newName: "Probability");

            migrationBuilder.RenameColumn(
                name: "RiskIncidence",
                table: "Risks",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "RiskDescription",
                table: "Risks",
                newName: "Level");

            migrationBuilder.RenameColumn(
                name: "RiskCreateDate",
                table: "Risks",
                newName: "CreateDate");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Risks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Incidence",
                table: "Risks",
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Risks");

            migrationBuilder.DropColumn(
                name: "Incidence",
                table: "Risks");

            migrationBuilder.DropColumn(
                name: "DefectStatus",
                table: "Defects");

            migrationBuilder.DropColumn(
                name: "DefectType",
                table: "Defects");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Risks",
                newName: "RiskSolution");

            migrationBuilder.RenameColumn(
                name: "Solution",
                table: "Risks",
                newName: "RiskProbability");

            migrationBuilder.RenameColumn(
                name: "Probability",
                table: "Risks",
                newName: "RiskName");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Risks",
                newName: "RiskIncidence");

            migrationBuilder.RenameColumn(
                name: "Level",
                table: "Risks",
                newName: "RiskDescription");

            migrationBuilder.RenameColumn(
                name: "CreateDate",
                table: "Risks",
                newName: "RiskCreateDate");

            migrationBuilder.AddColumn<int>(
                name: "RiskLevelId",
                table: "Risks",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RiskStatusId",
                table: "Risks",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DefectStatusId",
                table: "Defects",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DefectTypeId",
                table: "Defects",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Risks_RiskLevelId",
                table: "Risks",
                column: "RiskLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_Risks_RiskStatusId",
                table: "Risks",
                column: "RiskStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Defects_DefectStatusId",
                table: "Defects",
                column: "DefectStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Defects_DefectTypeId",
                table: "Defects",
                column: "DefectTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Defects_DefectStatuses_DefectStatusId",
                table: "Defects",
                column: "DefectStatusId",
                principalTable: "DefectStatuses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Defects_DefectTypes_DefectTypeId",
                table: "Defects",
                column: "DefectTypeId",
                principalTable: "DefectTypes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Risks_RiskLevels_RiskLevelId",
                table: "Risks",
                column: "RiskLevelId",
                principalTable: "RiskLevels",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Risks_RiskStatuses_RiskStatusId",
                table: "Risks",
                column: "RiskStatusId",
                principalTable: "RiskStatuses",
                principalColumn: "Id");
        }
    }
}
