using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectManagementSystem.Migrations
{
    public partial class kkk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fund_Projects_ProjectId",
                table: "Fund");

            migrationBuilder.DropForeignKey(
                name: "FK_FundChange_AspNetUsers_UserId",
                table: "FundChange");

            migrationBuilder.DropForeignKey(
                name: "FK_FundChange_Fund_FundId",
                table: "FundChange");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FundChange",
                table: "FundChange");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Fund",
                table: "Fund");

            migrationBuilder.RenameTable(
                name: "FundChange",
                newName: "FundChanges");

            migrationBuilder.RenameTable(
                name: "Fund",
                newName: "Funds");

            migrationBuilder.RenameIndex(
                name: "IX_FundChange_UserId",
                table: "FundChanges",
                newName: "IX_FundChanges_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_FundChange_FundId",
                table: "FundChanges",
                newName: "IX_FundChanges_FundId");

            migrationBuilder.RenameIndex(
                name: "IX_Fund_ProjectId",
                table: "Funds",
                newName: "IX_Funds_ProjectId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FundChanges",
                table: "FundChanges",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Funds",
                table: "Funds",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FundChanges_AspNetUsers_UserId",
                table: "FundChanges",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FundChanges_Funds_FundId",
                table: "FundChanges",
                column: "FundId",
                principalTable: "Funds",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Funds_Projects_ProjectId",
                table: "Funds",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FundChanges_AspNetUsers_UserId",
                table: "FundChanges");

            migrationBuilder.DropForeignKey(
                name: "FK_FundChanges_Funds_FundId",
                table: "FundChanges");

            migrationBuilder.DropForeignKey(
                name: "FK_Funds_Projects_ProjectId",
                table: "Funds");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Funds",
                table: "Funds");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FundChanges",
                table: "FundChanges");

            migrationBuilder.RenameTable(
                name: "Funds",
                newName: "Fund");

            migrationBuilder.RenameTable(
                name: "FundChanges",
                newName: "FundChange");

            migrationBuilder.RenameIndex(
                name: "IX_Funds_ProjectId",
                table: "Fund",
                newName: "IX_Fund_ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_FundChanges_UserId",
                table: "FundChange",
                newName: "IX_FundChange_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_FundChanges_FundId",
                table: "FundChange",
                newName: "IX_FundChange_FundId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Fund",
                table: "Fund",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FundChange",
                table: "FundChange",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Fund_Projects_ProjectId",
                table: "Fund",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FundChange_AspNetUsers_UserId",
                table: "FundChange",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FundChange_Fund_FundId",
                table: "FundChange",
                column: "FundId",
                principalTable: "Fund",
                principalColumn: "Id");
        }
    }
}
