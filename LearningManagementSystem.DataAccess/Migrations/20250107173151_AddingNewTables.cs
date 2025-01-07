using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearningManagementSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddingNewTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fee_students_StudentId",
                table: "Fee");

            migrationBuilder.DropForeignKey(
                name: "FK_Report_AspNetUsers_AppUserId",
                table: "Report");

            migrationBuilder.DropForeignKey(
                name: "FK_Report_AspNetUsers_ReportedUserId",
                table: "Report");

            migrationBuilder.DropForeignKey(
                name: "FK_Report_ReportOption_ReportOptionId",
                table: "Report");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ReportOption",
                table: "ReportOption");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Report",
                table: "Report");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Fee",
                table: "Fee");

            migrationBuilder.RenameTable(
                name: "ReportOption",
                newName: "reportOptions");

            migrationBuilder.RenameTable(
                name: "Report",
                newName: "reports");

            migrationBuilder.RenameTable(
                name: "Fee",
                newName: "fees");

            migrationBuilder.RenameIndex(
                name: "IX_Report_ReportOptionId",
                table: "reports",
                newName: "IX_reports_ReportOptionId");

            migrationBuilder.RenameIndex(
                name: "IX_Report_ReportedUserId",
                table: "reports",
                newName: "IX_reports_ReportedUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Report_AppUserId",
                table: "reports",
                newName: "IX_reports_AppUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Fee_StudentId",
                table: "fees",
                newName: "IX_fees_StudentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_reportOptions",
                table: "reportOptions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_reports",
                table: "reports",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_fees",
                table: "fees",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_fees_students_StudentId",
                table: "fees",
                column: "StudentId",
                principalTable: "students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_reports_AspNetUsers_AppUserId",
                table: "reports",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_reports_AspNetUsers_ReportedUserId",
                table: "reports",
                column: "ReportedUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_reports_reportOptions_ReportOptionId",
                table: "reports",
                column: "ReportOptionId",
                principalTable: "reportOptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_fees_students_StudentId",
                table: "fees");

            migrationBuilder.DropForeignKey(
                name: "FK_reports_AspNetUsers_AppUserId",
                table: "reports");

            migrationBuilder.DropForeignKey(
                name: "FK_reports_AspNetUsers_ReportedUserId",
                table: "reports");

            migrationBuilder.DropForeignKey(
                name: "FK_reports_reportOptions_ReportOptionId",
                table: "reports");

            migrationBuilder.DropPrimaryKey(
                name: "PK_reports",
                table: "reports");

            migrationBuilder.DropPrimaryKey(
                name: "PK_reportOptions",
                table: "reportOptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_fees",
                table: "fees");

            migrationBuilder.RenameTable(
                name: "reports",
                newName: "Report");

            migrationBuilder.RenameTable(
                name: "reportOptions",
                newName: "ReportOption");

            migrationBuilder.RenameTable(
                name: "fees",
                newName: "Fee");

            migrationBuilder.RenameIndex(
                name: "IX_reports_ReportOptionId",
                table: "Report",
                newName: "IX_Report_ReportOptionId");

            migrationBuilder.RenameIndex(
                name: "IX_reports_ReportedUserId",
                table: "Report",
                newName: "IX_Report_ReportedUserId");

            migrationBuilder.RenameIndex(
                name: "IX_reports_AppUserId",
                table: "Report",
                newName: "IX_Report_AppUserId");

            migrationBuilder.RenameIndex(
                name: "IX_fees_StudentId",
                table: "Fee",
                newName: "IX_Fee_StudentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Report",
                table: "Report",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReportOption",
                table: "ReportOption",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Fee",
                table: "Fee",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Fee_students_StudentId",
                table: "Fee",
                column: "StudentId",
                principalTable: "students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Report_AspNetUsers_AppUserId",
                table: "Report",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Report_AspNetUsers_ReportedUserId",
                table: "Report",
                column: "ReportedUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Report_ReportOption_ReportOptionId",
                table: "Report",
                column: "ReportOptionId",
                principalTable: "ReportOption",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
