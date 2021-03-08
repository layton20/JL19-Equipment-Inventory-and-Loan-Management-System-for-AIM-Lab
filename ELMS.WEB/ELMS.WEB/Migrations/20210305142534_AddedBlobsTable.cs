using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace ELMS.WEB.Migrations
{
    public partial class AddedBlobsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Blobs",
                columns: table => new
                {
                    UID = table.Column<Guid>(nullable: false),
                    CreatedTimestamp = table.Column<DateTime>(nullable: false),
                    AmendedTimestamp = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Path = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blobs", x => x.UID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Blobs");
        }
    }
}