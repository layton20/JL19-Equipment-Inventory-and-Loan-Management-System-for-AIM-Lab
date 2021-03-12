using Microsoft.EntityFrameworkCore.Migrations;

namespace ELMS.WEB.Migrations
{
    public partial class AddedExtraFieldsForEmailTemplateTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Footer",
                table: "EmailTemplates",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Header",
                table: "EmailTemplates",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Subheader",
                table: "EmailTemplates",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Footer",
                table: "EmailTemplates");

            migrationBuilder.DropColumn(
                name: "Header",
                table: "EmailTemplates");

            migrationBuilder.DropColumn(
                name: "Subheader",
                table: "EmailTemplates");
        }
    }
}
