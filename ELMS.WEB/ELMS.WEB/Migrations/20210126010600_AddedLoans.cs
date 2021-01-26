using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ELMS.WEB.Migrations
{
    public partial class AddedLoans : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Loans",
                columns: table => new
                {
                    UID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LoanerUID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoaneeUID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoaneeEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FromTimestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiryTimestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    AcceptedTermsAndConditions = table.Column<bool>(type: "bit", nullable: false),
                    LoaneeId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedTimestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AmendedTimestamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loans", x => x.UID);
                    table.ForeignKey(
                        name: "FK_Loans_AspNetUsers_LoaneeId",
                        column: x => x.LoaneeId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LoanEquipmentList",
                columns: table => new
                {
                    UID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoanUID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EquipmentUID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedTimestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AmendedTimestamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoanEquipmentList", x => x.UID);
                    table.ForeignKey(
                        name: "FK_LoanEquipmentList_Equipment_EquipmentUID",
                        column: x => x.EquipmentUID,
                        principalTable: "Equipment",
                        principalColumn: "UID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LoanEquipmentList_Loans_LoanUID",
                        column: x => x.LoanUID,
                        principalTable: "Loans",
                        principalColumn: "UID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LoanEquipmentList_EquipmentUID",
                table: "LoanEquipmentList",
                column: "EquipmentUID");

            migrationBuilder.CreateIndex(
                name: "IX_LoanEquipmentList_LoanUID",
                table: "LoanEquipmentList",
                column: "LoanUID");

            migrationBuilder.CreateIndex(
                name: "IX_Loans_LoaneeId",
                table: "Loans",
                column: "LoaneeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LoanEquipmentList");

            migrationBuilder.DropTable(
                name: "Loans");
        }
    }
}
