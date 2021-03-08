using Microsoft.EntityFrameworkCore.Migrations;

namespace ELMS.WEB.Migrations
{
    public partial class AddedForeignKeysForEquipmentBlobTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_EquipmentBlobs_BlobUID",
                table: "EquipmentBlobs",
                column: "BlobUID");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentBlobs_EquipmentUID",
                table: "EquipmentBlobs",
                column: "EquipmentUID");

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentBlobs_Blobs_BlobUID",
                table: "EquipmentBlobs",
                column: "BlobUID",
                principalTable: "Blobs",
                principalColumn: "UID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentBlobs_Equipment_EquipmentUID",
                table: "EquipmentBlobs",
                column: "EquipmentUID",
                principalTable: "Equipment",
                principalColumn: "UID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentBlobs_Blobs_BlobUID",
                table: "EquipmentBlobs");

            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentBlobs_Equipment_EquipmentUID",
                table: "EquipmentBlobs");

            migrationBuilder.DropIndex(
                name: "IX_EquipmentBlobs_BlobUID",
                table: "EquipmentBlobs");

            migrationBuilder.DropIndex(
                name: "IX_EquipmentBlobs_EquipmentUID",
                table: "EquipmentBlobs");
        }
    }
}