using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BataCMS.Migrations
{
    public partial class addMemberCertificate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActiveLeases_HPAFacilities_HPAFacilityId",
                table: "ActiveLeases");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ActiveLeases",
                table: "ActiveLeases");

            migrationBuilder.DropIndex(
                name: "IX_ActiveLeases_HPAFacilityId",
                table: "ActiveLeases");

            migrationBuilder.DropColumn(
                name: "ActiveLeaseId",
                table: "ActiveLeases");

            migrationBuilder.AddColumn<int>(
                name: "MemberCertificateId",
                table: "ActiveLeases",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "MemberUserId",
                table: "ActiveLeases",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ValidFrom",
                table: "ActiveLeases",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ValidTo",
                table: "ActiveLeases",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_ActiveLeases",
                table: "ActiveLeases",
                column: "MemberCertificateId");

            migrationBuilder.CreateIndex(
                name: "IX_ActiveLeases_MemberUserId",
                table: "ActiveLeases",
                column: "MemberUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ActiveLeases_AspNetUsers_MemberUserId",
                table: "ActiveLeases",
                column: "MemberUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActiveLeases_AspNetUsers_MemberUserId",
                table: "ActiveLeases");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ActiveLeases",
                table: "ActiveLeases");

            migrationBuilder.DropIndex(
                name: "IX_ActiveLeases_MemberUserId",
                table: "ActiveLeases");

            migrationBuilder.DropColumn(
                name: "MemberCertificateId",
                table: "ActiveLeases");

            migrationBuilder.DropColumn(
                name: "MemberUserId",
                table: "ActiveLeases");

            migrationBuilder.DropColumn(
                name: "ValidFrom",
                table: "ActiveLeases");

            migrationBuilder.DropColumn(
                name: "ValidTo",
                table: "ActiveLeases");

            migrationBuilder.AddColumn<int>(
                name: "ActiveLeaseId",
                table: "ActiveLeases",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ActiveLeases",
                table: "ActiveLeases",
                column: "ActiveLeaseId");

            migrationBuilder.CreateIndex(
                name: "IX_ActiveLeases_HPAFacilityId",
                table: "ActiveLeases",
                column: "HPAFacilityId");

            migrationBuilder.AddForeignKey(
                name: "FK_ActiveLeases_HPAFacilities_HPAFacilityId",
                table: "ActiveLeases",
                column: "HPAFacilityId",
                principalTable: "HPAFacilities",
                principalColumn: "HPAFacilityId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
