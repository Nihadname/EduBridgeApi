using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearningManagementSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class NewTargatedUserInReport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReportedUserId",
                table: "Report",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Report_ReportedUserId",
                table: "Report",
                column: "ReportedUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Report_AspNetUsers_ReportedUserId",
                table: "Report",
                column: "ReportedUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Report_AspNetUsers_ReportedUserId",
                table: "Report");

            migrationBuilder.DropIndex(
                name: "IX_Report_ReportedUserId",
                table: "Report");

            migrationBuilder.DropColumn(
                name: "ReportedUserId",
                table: "Report");
        }
    }
}
