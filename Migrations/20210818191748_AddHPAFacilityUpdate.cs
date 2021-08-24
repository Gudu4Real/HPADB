using Microsoft.EntityFrameworkCore.Migrations;

namespace BataCMS.Migrations
{
    public partial class AddHPAFacilityUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HPAFacilityId",
                table: "Leases",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "HPAFacilityId",
                table: "Image",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HPAFacilityId",
                table: "ActiveLeases",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Image_HPAFacilityId",
                table: "Image",
                column: "HPAFacilityId");

            migrationBuilder.CreateIndex(
                name: "IX_ActiveLeases_HPAFacilityId",
                table: "ActiveLeases",
                column: "HPAFacilityId");

            migrationBuilder.AddForeignKey(
                name: "FK_ActiveLeases_RentalAssets_HPAFacilityId",
                table: "ActiveLeases",
                column: "HPAFacilityId",
                principalTable: "RentalAssets",
                principalColumn: "HPAFacilityId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Image_RentalAssets_HPAFacilityId",
                table: "Image",
                column: "HPAFacilityId",
                principalTable: "RentalAssets",
                principalColumn: "HPAFacilityId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActiveLeases_RentalAssets_HPAFacilityId",
                table: "ActiveLeases");

            migrationBuilder.DropForeignKey(
                name: "FK_Image_RentalAssets_HPAFacilityId",
                table: "Image");

            migrationBuilder.DropIndex(
                name: "IX_Image_HPAFacilityId",
                table: "Image");

            migrationBuilder.DropIndex(
                name: "IX_ActiveLeases_HPAFacilityId",
                table: "ActiveLeases");

            migrationBuilder.DropColumn(
                name: "HPAFacilityId",
                table: "Leases");

            migrationBuilder.DropColumn(
                name: "HPAFacilityId",
                table: "Image");

            migrationBuilder.DropColumn(
                name: "HPAFacilityId",
                table: "ActiveLeases");
        }
    }
}
