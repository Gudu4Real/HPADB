using Microsoft.EntityFrameworkCore.Migrations;

namespace BataCMS.Migrations
{
    public partial class MemberFacilities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActiveLeases_RentalAssets_HPAFacilityId",
                table: "ActiveLeases");

            migrationBuilder.DropForeignKey(
                name: "FK_Image_RentalAssets_HPAFacilityId",
                table: "Image");

            migrationBuilder.DropForeignKey(
                name: "FK_RentalAssets_Categories_CategoryId",
                table: "RentalAssets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RentalAssets",
                table: "RentalAssets");

            migrationBuilder.RenameTable(
                name: "RentalAssets",
                newName: "HPAFacilities");

            migrationBuilder.RenameIndex(
                name: "IX_RentalAssets_CategoryId",
                table: "HPAFacilities",
                newName: "IX_HPAFacilities_CategoryId");

            migrationBuilder.AddColumn<string>(
                name: "MemberUserId",
                table: "HPAFacilities",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_HPAFacilities",
                table: "HPAFacilities",
                column: "HPAFacilityId");

            migrationBuilder.CreateIndex(
                name: "IX_HPAFacilities_MemberUserId",
                table: "HPAFacilities",
                column: "MemberUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ActiveLeases_HPAFacilities_HPAFacilityId",
                table: "ActiveLeases",
                column: "HPAFacilityId",
                principalTable: "HPAFacilities",
                principalColumn: "HPAFacilityId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HPAFacilities_Categories_CategoryId",
                table: "HPAFacilities",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HPAFacilities_AspNetUsers_MemberUserId",
                table: "HPAFacilities",
                column: "MemberUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Image_HPAFacilities_HPAFacilityId",
                table: "Image",
                column: "HPAFacilityId",
                principalTable: "HPAFacilities",
                principalColumn: "HPAFacilityId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActiveLeases_HPAFacilities_HPAFacilityId",
                table: "ActiveLeases");

            migrationBuilder.DropForeignKey(
                name: "FK_HPAFacilities_Categories_CategoryId",
                table: "HPAFacilities");

            migrationBuilder.DropForeignKey(
                name: "FK_HPAFacilities_AspNetUsers_MemberUserId",
                table: "HPAFacilities");

            migrationBuilder.DropForeignKey(
                name: "FK_Image_HPAFacilities_HPAFacilityId",
                table: "Image");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HPAFacilities",
                table: "HPAFacilities");

            migrationBuilder.DropIndex(
                name: "IX_HPAFacilities_MemberUserId",
                table: "HPAFacilities");

            migrationBuilder.DropColumn(
                name: "MemberUserId",
                table: "HPAFacilities");

            migrationBuilder.RenameTable(
                name: "HPAFacilities",
                newName: "RentalAssets");

            migrationBuilder.RenameIndex(
                name: "IX_HPAFacilities_CategoryId",
                table: "RentalAssets",
                newName: "IX_RentalAssets_CategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RentalAssets",
                table: "RentalAssets",
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

            migrationBuilder.AddForeignKey(
                name: "FK_RentalAssets_Categories_CategoryId",
                table: "RentalAssets",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
