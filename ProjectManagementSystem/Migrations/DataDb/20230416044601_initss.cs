using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectManagementSystem.Migrations.DataDb
{
    public partial class initss : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SelectFunctionary",
                table: "Projects",
                newName: "Functionary");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Functionary",
                table: "Projects",
                newName: "SelectFunctionary");
        }
    }
}
