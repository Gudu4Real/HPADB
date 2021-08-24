using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BataCMS.Migrations
{
    public partial class AddHPAFacility : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActiveLeases_RentalAssets_RentalAssetId",
                table: "ActiveLeases");

            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_RentalAssets_RentalAssetId",
                table: "Invoices");

            migrationBuilder.DropForeignKey(
                name: "FK_Leases_RentalAssets_RentalAssetId",
                table: "Leases");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RentalAssets",
                table: "RentalAssets");

            migrationBuilder.DropIndex(
                name: "IX_Leases_RentalAssetId",
                table: "Leases");

            migrationBuilder.DropIndex(
                name: "IX_Invoices_RentalAssetId",
                table: "Invoices");

            migrationBuilder.DropIndex(
                name: "IX_ActiveLeases_RentalAssetId",
                table: "ActiveLeases");

            migrationBuilder.DropColumn(
                name: "RentalAssetId",
                table: "RentalAssets");

            migrationBuilder.DropColumn(
                name: "RentalAssetId",
                table: "Leases");

            migrationBuilder.DropColumn(
                name: "AmountPaid",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "RentalAssetId",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "RentalAssetId",
                table: "ActiveLeases");

            migrationBuilder.AddColumn<int>(
                name: "HPAFacilityId",
                table: "RentalAssets",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<decimal>(
                name: "AmountDue",
                table: "Invoices",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Invoices",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_RentalAssets",
                table: "RentalAssets",
                column: "HPAFacilityId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_RentalAssets",
                table: "RentalAssets");

            migrationBuilder.DropColumn(
                name: "HPAFacilityId",
                table: "RentalAssets");

            migrationBuilder.DropColumn(
                name: "AmountDue",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "Invoices");

            migrationBuilder.AddColumn<int>(
                name: "RentalAssetId",
                table: "RentalAssets",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "RentalAssetId",
                table: "Leases",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "AmountPaid",
                table: "Invoices",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "RentalAssetId",
                table: "Invoices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RentalAssetId",
                table: "ActiveLeases",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RentalAssets",
                table: "RentalAssets",
                column: "RentalAssetId");

            migrationBuilder.CreateIndex(
                name: "IX_Leases_RentalAssetId",
                table: "Leases",
                column: "RentalAssetId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_RentalAssetId",
                table: "Invoices",
                column: "RentalAssetId");

            migrationBuilder.CreateIndex(
                name: "IX_ActiveLeases_RentalAssetId",
                table: "ActiveLeases",
                column: "RentalAssetId");

            migrationBuilder.AddForeignKey(
                name: "FK_ActiveLeases_RentalAssets_RentalAssetId",
                table: "ActiveLeases",
                column: "RentalAssetId",
                principalTable: "RentalAssets",
                principalColumn: "RentalAssetId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_RentalAssets_RentalAssetId",
                table: "Invoices",
                column: "RentalAssetId",
                principalTable: "RentalAssets",
                principalColumn: "RentalAssetId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Leases_RentalAssets_RentalAssetId",
                table: "Leases",
                column: "RentalAssetId",
                principalTable: "RentalAssets",
                principalColumn: "RentalAssetId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
