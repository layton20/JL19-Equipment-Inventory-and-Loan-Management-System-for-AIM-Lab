using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ELMS.WEB.Migrations
{
    public partial class AddedEmailScheduleTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmailSchedules",
                columns: table => new
                {
                    UID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmailTemplateUID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RecipientEmailAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    SendTimestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedTimestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AmendedTimestamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailSchedules", x => x.UID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmailSchedules");
        }
    }
}
