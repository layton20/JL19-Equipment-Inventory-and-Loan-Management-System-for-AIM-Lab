using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace ELMS.WEB.Migrations
{
    public partial class UpdatedEmailScheduleTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "EmailSchedules");

            migrationBuilder.AddColumn<int>(
                name: "EmailType",
                table: "EmailSchedules",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Sent",
                table: "EmailSchedules",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "EmailScheduleParameters",
                columns: table => new
                {
                    UID = table.Column<Guid>(nullable: false),
                    CreatedTimestamp = table.Column<DateTime>(nullable: false),
                    AmendedTimestamp = table.Column<DateTime>(nullable: false),
                    EmailScheduleUID = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailScheduleParameters", x => x.UID);
                    table.ForeignKey(
                        name: "FK_EmailScheduleParameters_EmailSchedules_EmailScheduleUID",
                        column: x => x.EmailScheduleUID,
                        principalTable: "EmailSchedules",
                        principalColumn: "UID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmailScheduleParameters_EmailScheduleUID",
                table: "EmailScheduleParameters",
                column: "EmailScheduleUID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmailScheduleParameters");

            migrationBuilder.DropColumn(
                name: "EmailType",
                table: "EmailSchedules");

            migrationBuilder.DropColumn(
                name: "Sent",
                table: "EmailSchedules");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "EmailSchedules",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}