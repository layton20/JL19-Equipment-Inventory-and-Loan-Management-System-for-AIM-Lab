using Microsoft.EntityFrameworkCore.Migrations;

namespace ELMS.WEB.Migrations
{
    public partial class AddedLoanerEmailFieldToLoanTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Loans_AspNetUsers_LoanerUID",
                table: "Loans");

            migrationBuilder.DropIndex(
                name: "IX_Loans_LoanerUID",
                table: "Loans");

            migrationBuilder.DropColumn(
                name: "LoaneeUID",
                table: "Loans");

            migrationBuilder.DropColumn(
                name: "LoanerUID",
                table: "Loans");

            migrationBuilder.AddColumn<string>(
                name: "LoanerEmail",
                table: "Loans",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LoanerEmail",
                table: "Loans");

            migrationBuilder.AddColumn<string>(
                name: "LoaneeUID",
                table: "Loans",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LoanerUID",
                table: "Loans",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Loans_LoanerUID",
                table: "Loans",
                column: "LoanerUID");

            migrationBuilder.AddForeignKey(
                name: "FK_Loans_AspNetUsers_LoanerUID",
                table: "Loans",
                column: "LoanerUID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
