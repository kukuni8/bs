using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectManagementSystem.Migrations
{
    public partial class dasd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Defects_AspNetUsers_FunctionaryId",
                table: "Defects");

            migrationBuilder.AlterColumn<string>(
                name: "FunctionaryId",
                table: "Defects",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_Defects_AspNetUsers_FunctionaryId",
                table: "Defects",
                column: "FunctionaryId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Defects_AspNetUsers_FunctionaryId",
                table: "Defects");

            migrationBuilder.AlterColumn<string>(
                name: "FunctionaryId",
                table: "Defects",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Defects_AspNetUsers_FunctionaryId",
                table: "Defects",
                column: "FunctionaryId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
