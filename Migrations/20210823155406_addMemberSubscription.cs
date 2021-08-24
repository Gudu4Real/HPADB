using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BataCMS.Migrations
{
    public partial class addMemberSubscription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActiveLeases_Leases_LeaseId",
                table: "ActiveLeases");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Leases_LeaseId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_LeaseId",
                table: "Transactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Leases",
                table: "Leases");

            migrationBuilder.DropIndex(
                name: "IX_ActiveLeases_LeaseId",
                table: "ActiveLeases");

            migrationBuilder.DropColumn(
                name: "LeaseId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "LeaseId",
                table: "Leases");

            migrationBuilder.DropColumn(
                name: "leaseFrom",
                table: "Leases");

            migrationBuilder.DropColumn(
                name: "leaseTo",
                table: "Leases");

            migrationBuilder.DropColumn(
                name: "LeaseId",
                table: "ActiveLeases");

            migrationBuilder.AddColumn<int>(
                name: "LeaseMemberSubscriptionId",
                table: "Transactions",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MemberSubscriptionId",
                table: "Leases",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<DateTime>(
                name: "From",
                table: "Leases",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "To",
                table: "Leases",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "LeaseMemberSubscriptionId",
                table: "ActiveLeases",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Leases",
                table: "Leases",
                column: "MemberSubscriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_LeaseMemberSubscriptionId",
                table: "Transactions",
                column: "LeaseMemberSubscriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_ActiveLeases_LeaseMemberSubscriptionId",
                table: "ActiveLeases",
                column: "LeaseMemberSubscriptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_ActiveLeases_Leases_LeaseMemberSubscriptionId",
                table: "ActiveLeases",
                column: "LeaseMemberSubscriptionId",
                principalTable: "Leases",
                principalColumn: "MemberSubscriptionId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Leases_LeaseMemberSubscriptionId",
                table: "Transactions",
                column: "LeaseMemberSubscriptionId",
                principalTable: "Leases",
                principalColumn: "MemberSubscriptionId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActiveLeases_Leases_LeaseMemberSubscriptionId",
                table: "ActiveLeases");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Leases_LeaseMemberSubscriptionId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_LeaseMemberSubscriptionId",
                table: "Transactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Leases",
                table: "Leases");

            migrationBuilder.DropIndex(
                name: "IX_ActiveLeases_LeaseMemberSubscriptionId",
                table: "ActiveLeases");

            migrationBuilder.DropColumn(
                name: "LeaseMemberSubscriptionId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "MemberSubscriptionId",
                table: "Leases");

            migrationBuilder.DropColumn(
                name: "From",
                table: "Leases");

            migrationBuilder.DropColumn(
                name: "To",
                table: "Leases");

            migrationBuilder.DropColumn(
                name: "LeaseMemberSubscriptionId",
                table: "ActiveLeases");

            migrationBuilder.AddColumn<int>(
                name: "LeaseId",
                table: "Transactions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LeaseId",
                table: "Leases",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<DateTime>(
                name: "leaseFrom",
                table: "Leases",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "leaseTo",
                table: "Leases",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "LeaseId",
                table: "ActiveLeases",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Leases",
                table: "Leases",
                column: "LeaseId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_LeaseId",
                table: "Transactions",
                column: "LeaseId");

            migrationBuilder.CreateIndex(
                name: "IX_ActiveLeases_LeaseId",
                table: "ActiveLeases",
                column: "LeaseId");

            migrationBuilder.AddForeignKey(
                name: "FK_ActiveLeases_Leases_LeaseId",
                table: "ActiveLeases",
                column: "LeaseId",
                principalTable: "Leases",
                principalColumn: "LeaseId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Leases_LeaseId",
                table: "Transactions",
                column: "LeaseId",
                principalTable: "Leases",
                principalColumn: "LeaseId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
