using Microsoft.EntityFrameworkCore.Migrations;

namespace BataCMS.Migrations
{
    public partial class addMemberCertificateUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActiveLeases_AspNetUsers_MemberUserId",
                table: "ActiveLeases");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ActiveLeases",
                table: "ActiveLeases");

            migrationBuilder.RenameTable(
                name: "ActiveLeases",
                newName: "MemberCertificates");

            migrationBuilder.RenameIndex(
                name: "IX_ActiveLeases_MemberUserId",
                table: "MemberCertificates",
                newName: "IX_MemberCertificates_MemberUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MemberCertificates",
                table: "MemberCertificates",
                column: "MemberCertificateId");

            migrationBuilder.AddForeignKey(
                name: "FK_MemberCertificates_AspNetUsers_MemberUserId",
                table: "MemberCertificates",
                column: "MemberUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MemberCertificates_AspNetUsers_MemberUserId",
                table: "MemberCertificates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MemberCertificates",
                table: "MemberCertificates");

            migrationBuilder.RenameTable(
                name: "MemberCertificates",
                newName: "ActiveLeases");

            migrationBuilder.RenameIndex(
                name: "IX_MemberCertificates_MemberUserId",
                table: "ActiveLeases",
                newName: "IX_ActiveLeases_MemberUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ActiveLeases",
                table: "ActiveLeases",
                column: "MemberCertificateId");

            migrationBuilder.AddForeignKey(
                name: "FK_ActiveLeases_AspNetUsers_MemberUserId",
                table: "ActiveLeases",
                column: "MemberUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
