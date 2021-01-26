using Microsoft.EntityFrameworkCore.Migrations;

namespace ELMS.WEB.Migrations
{
    public partial class UpdatedNoteTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notes_Equipment_EquipmentID",
                table: "Notes");

            migrationBuilder.RenameColumn(
                name: "EquipmentID",
                table: "Notes",
                newName: "EquipmentUID");

            migrationBuilder.RenameIndex(
                name: "IX_Notes_EquipmentID",
                table: "Notes",
                newName: "IX_Notes_EquipmentUID");

            migrationBuilder.AddForeignKey(
                name: "FK_Notes_Equipment_EquipmentUID",
                table: "Notes",
                column: "EquipmentUID",
                principalTable: "Equipment",
                principalColumn: "UID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notes_Equipment_EquipmentUID",
                table: "Notes");

            migrationBuilder.RenameColumn(
                name: "EquipmentUID",
                table: "Notes",
                newName: "EquipmentID");

            migrationBuilder.RenameIndex(
                name: "IX_Notes_EquipmentUID",
                table: "Notes",
                newName: "IX_Notes_EquipmentID");

            migrationBuilder.AddForeignKey(
                name: "FK_Notes_Equipment_EquipmentID",
                table: "Notes",
                column: "EquipmentID",
                principalTable: "Equipment",
                principalColumn: "UID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}