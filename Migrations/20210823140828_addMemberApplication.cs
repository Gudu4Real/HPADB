using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BataCMS.Migrations
{
    public partial class addMemberApplication : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VendorApplications");

            migrationBuilder.CreateTable(
                name: "MemberApplications",
                columns: table => new
                {
                    MemberApplicationId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicantName = table.Column<string>(nullable: true),
                    ApplicantId = table.Column<string>(nullable: false),
                    IdProofUrl = table.Column<string>(nullable: true),
                    ResidencyProofUrl = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    ApplicationDate = table.Column<DateTime>(nullable: false),
                    RejectMessage = table.Column<string>(nullable: true),
                    ApplicationUserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberApplications", x => x.MemberApplicationId);
                    table.ForeignKey(
                        name: "FK_MemberApplications_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MemberApplications_ApplicationUserId",
                table: "MemberApplications",
                column: "ApplicationUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MemberApplications");

            migrationBuilder.CreateTable(
                name: "VendorApplications",
                columns: table => new
                {
                    VendorApplicationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicantId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApplicantName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApplicationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IdProofUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RejectMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResidencyProofUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VendorApplications", x => x.VendorApplicationId);
                    table.ForeignKey(
                        name: "FK_VendorApplications_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VendorApplications_ApplicationUserId",
                table: "VendorApplications",
                column: "ApplicationUserId");
        }
    }
}
