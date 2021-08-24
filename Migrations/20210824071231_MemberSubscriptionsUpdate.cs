using Microsoft.EntityFrameworkCore.Migrations;

namespace BataCMS.Migrations
{
    public partial class MemberSubscriptionsUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropIndex(
                name: "IX_ActiveLeases_LeaseMemberSubscriptionId",
                table: "ActiveLeases");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Leases",
                table: "Leases");

            migrationBuilder.DropColumn(
                name: "LeaseMemberSubscriptionId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "LeaseMemberSubscriptionId",
                table: "ActiveLeases");

            migrationBuilder.RenameTable(
                name: "Leases",
                newName: "MemberSubscriptions");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MemberSubscriptions",
                table: "MemberSubscriptions",
                column: "MemberSubscriptionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MemberSubscriptions",
                table: "MemberSubscriptions");

            migrationBuilder.RenameTable(
                name: "MemberSubscriptions",
                newName: "Leases");

            migrationBuilder.AddColumn<int>(
                name: "LeaseMemberSubscriptionId",
                table: "Transactions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LeaseMemberSubscriptionId",
                table: "ActiveLeases",
                type: "int",
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
    }
}
