using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace ELMS.WEB.Migrations
{
    public partial class AddedLoanExtensionTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LoanExtensions",
                columns: table => new
                {
                    UID = table.Column<Guid>(nullable: false),
                    CreatedTimestamp = table.Column<DateTime>(nullable: false),
                    AmendedTimestamp = table.Column<DateTime>(nullable: false),
                    LoanUID = table.Column<Guid>(nullable: false),
                    ExtenderEmail = table.Column<string>(nullable: false),
                    PreviousExpiryDate = table.Column<DateTime>(nullable: false),
                    NewExpiryDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoanExtensions", x => x.UID);
                    table.ForeignKey(
                        name: "FK_LoanExtensions_Loans_LoanUID",
                        column: x => x.LoanUID,
                        principalTable: "Loans",
                        principalColumn: "UID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LoanExtensions_LoanUID",
                table: "LoanExtensions",
                column: "LoanUID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LoanExtensions");
        }
    }
}