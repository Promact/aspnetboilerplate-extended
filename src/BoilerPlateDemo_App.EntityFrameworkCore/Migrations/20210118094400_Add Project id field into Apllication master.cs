using Microsoft.EntityFrameworkCore.Migrations;

namespace BoilerPlateDemo_App.Migrations
{
    public partial class AddProjectidfieldintoApllicationmaster : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "Application",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "Application");
        }
    }
}
