using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ELMS.WEB.Migrations
{
    public partial class UpdatedTypesForLoanTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Loans_AspNetUsers_LoaneeId",
                table: "Loans");

            migrationBuilder.DropIndex(
                name: "IX_Loans_LoaneeId",
                table: "Loans");

            migrationBuilder.DropColumn(
                name: "LoaneeId",
                table: "Loans");

            migrationBuilder.AlterColumn<string>(
                name: "LoanerUID",
                table: "Loans",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<string>(
                name: "LoaneeUID",
                table: "Loans",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Loans_AspNetUsers_LoanerUID",
                table: "Loans");

            migrationBuilder.DropIndex(
                name: "IX_Loans_LoanerUID",
                table: "Loans");

            migrationBuilder.AlterColumn<Guid>(
                name: "LoanerUID",
                table: "Loans",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "LoaneeUID",
                table: "Loans",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LoaneeId",
                table: "Loans",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Loans_LoaneeId",
                table: "Loans",
                column: "LoaneeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Loans_AspNetUsers_LoaneeId",
                table: "Loans",
                column: "LoaneeId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
