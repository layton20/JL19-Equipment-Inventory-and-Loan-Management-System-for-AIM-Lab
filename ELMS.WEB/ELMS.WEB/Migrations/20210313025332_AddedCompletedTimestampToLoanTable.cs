using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace ELMS.WEB.Migrations
{
    public partial class AddedCompletedTimestampToLoanTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CompletedTimestamp",
                table: "Loans",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompletedTimestamp",
                table: "Loans");
        }
    }
}