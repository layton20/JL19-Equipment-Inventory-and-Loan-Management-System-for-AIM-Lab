using Microsoft.EntityFrameworkCore.Migrations;

namespace ELMS.WEB.Migrations
{
    public partial class AddedActiveToBlacklistTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "Blacklists",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Active",
                table: "Blacklists");
        }
    }
}
