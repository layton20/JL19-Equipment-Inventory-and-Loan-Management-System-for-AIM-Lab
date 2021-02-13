using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ELMS.WEB.Migrations
{
    public partial class AddedBlacklistTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Blacklists",
                columns: table => new
                {
                    UID = table.Column<Guid>(nullable: false),
                    CreatedTimestamp = table.Column<DateTime>(nullable: false),
                    AmendedTimestamp = table.Column<DateTime>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    Reason = table.Column<string>(nullable: false),
                    Type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blacklists", x => x.UID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Blacklists");
        }
    }
}
