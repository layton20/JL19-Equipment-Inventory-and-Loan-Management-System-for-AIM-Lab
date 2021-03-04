using Microsoft.EntityFrameworkCore.Migrations;

namespace ELMS.WEB.Migrations
{
    public partial class AddedDescriptionToConfigurationTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Configurations",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Configurations");
        }
    }
}
