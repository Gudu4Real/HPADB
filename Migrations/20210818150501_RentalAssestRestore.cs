using Microsoft.EntityFrameworkCore.Migrations;

namespace BataCMS.Migrations
{
    public partial class RentalAssestRestore : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Image_RentalAssets_RentalAssetId",
                table: "Image");

            migrationBuilder.DropIndex(
                name: "IX_Image_RentalAssetId",
                table: "Image");

            migrationBuilder.DropColumn(
                name: "HPAFacilityId",
                table: "RentalAssets");

            migrationBuilder.DropColumn(
                name: "RentalAssetId",
                table: "Image");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HPAFacilityId",
                table: "RentalAssets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RentalAssetId",
                table: "Image",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Image_RentalAssetId",
                table: "Image",
                column: "RentalAssetId");

            migrationBuilder.AddForeignKey(
                name: "FK_Image_RentalAssets_RentalAssetId",
                table: "Image",
                column: "RentalAssetId",
                principalTable: "RentalAssets",
                principalColumn: "RentalAssetId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
