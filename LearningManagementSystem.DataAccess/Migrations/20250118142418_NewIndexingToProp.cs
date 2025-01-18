using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearningManagementSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class NewIndexingToProp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_teachers_CreatedTime",
                table: "teachers",
                column: "CreatedTime");

            migrationBuilder.CreateIndex(
                name: "IX_students_CreatedTime",
                table: "students",
                column: "CreatedTime");

            migrationBuilder.CreateIndex(
                name: "IX_requestToRegister_CreatedTime",
                table: "requestToRegister",
                column: "CreatedTime");

            migrationBuilder.CreateIndex(
                name: "IX_reports_CreatedTime",
                table: "reports",
                column: "CreatedTime");

            migrationBuilder.CreateIndex(
                name: "IX_reportOptions_CreatedTime",
                table: "reportOptions",
                column: "CreatedTime");

            migrationBuilder.CreateIndex(
                name: "IX_quizResults_CreatedTime",
                table: "quizResults",
                column: "CreatedTime");

            migrationBuilder.CreateIndex(
                name: "IX_parents_CreatedTime",
                table: "parents",
                column: "CreatedTime");

            migrationBuilder.CreateIndex(
                name: "IX_notes_CreatedTime",
                table: "notes",
                column: "CreatedTime");

            migrationBuilder.CreateIndex(
                name: "IX_lessonsVideo_CreatedTime",
                table: "lessonsVideo",
                column: "CreatedTime");

            migrationBuilder.CreateIndex(
                name: "IX_lessonsStudents_CreatedTime",
                table: "lessonsStudents",
                column: "CreatedTime");

            migrationBuilder.CreateIndex(
                name: "IX_lessonsMaterial_CreatedTime",
                table: "lessonsMaterial",
                column: "CreatedTime");

            migrationBuilder.CreateIndex(
                name: "IX_lessonQuizzes_CreatedTime",
                table: "lessonQuizzes",
                column: "CreatedTime");

            migrationBuilder.CreateIndex(
                name: "IX_fees_CreatedTime",
                table: "fees",
                column: "CreatedTime");

            migrationBuilder.CreateIndex(
                name: "IX_fees_PaidDate",
                table: "fees",
                column: "PaidDate");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_CreatedTime",
                table: "Courses",
                column: "CreatedTime");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CreatedTime",
                table: "AspNetUsers",
                column: "CreatedTime");

            migrationBuilder.CreateIndex(
                name: "IX_addresses_CreatedTime",
                table: "addresses",
                column: "CreatedTime");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_teachers_CreatedTime",
                table: "teachers");

            migrationBuilder.DropIndex(
                name: "IX_students_CreatedTime",
                table: "students");

            migrationBuilder.DropIndex(
                name: "IX_requestToRegister_CreatedTime",
                table: "requestToRegister");

            migrationBuilder.DropIndex(
                name: "IX_reports_CreatedTime",
                table: "reports");

            migrationBuilder.DropIndex(
                name: "IX_reportOptions_CreatedTime",
                table: "reportOptions");

            migrationBuilder.DropIndex(
                name: "IX_quizResults_CreatedTime",
                table: "quizResults");

            migrationBuilder.DropIndex(
                name: "IX_parents_CreatedTime",
                table: "parents");

            migrationBuilder.DropIndex(
                name: "IX_notes_CreatedTime",
                table: "notes");

            migrationBuilder.DropIndex(
                name: "IX_lessonsVideo_CreatedTime",
                table: "lessonsVideo");

            migrationBuilder.DropIndex(
                name: "IX_lessonsStudents_CreatedTime",
                table: "lessonsStudents");

            migrationBuilder.DropIndex(
                name: "IX_lessonsMaterial_CreatedTime",
                table: "lessonsMaterial");

            migrationBuilder.DropIndex(
                name: "IX_lessonQuizzes_CreatedTime",
                table: "lessonQuizzes");

            migrationBuilder.DropIndex(
                name: "IX_fees_CreatedTime",
                table: "fees");

            migrationBuilder.DropIndex(
                name: "IX_fees_PaidDate",
                table: "fees");

            migrationBuilder.DropIndex(
                name: "IX_Courses_CreatedTime",
                table: "Courses");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_CreatedTime",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_addresses_CreatedTime",
                table: "addresses");
        }
    }
}
