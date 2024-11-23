using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearningManagementSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ApplyToCourse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LessonMaterial_lessons_LessonId",
                table: "LessonMaterial");

            migrationBuilder.DropForeignKey(
                name: "FK_LessonQuiz_lessons_LessonId",
                table: "LessonQuiz");

            migrationBuilder.DropForeignKey(
                name: "FK_LessonVideo_lessons_LessonId",
                table: "LessonVideo");

            migrationBuilder.DropForeignKey(
                name: "FK_Note_AspNetUsers_AppUserId",
                table: "Note");

            migrationBuilder.DropForeignKey(
                name: "FK_QuizResult_LessonQuiz_QuizId",
                table: "QuizResult");

            migrationBuilder.DropForeignKey(
                name: "FK_QuizResult_lessonsStudents_LessonStudentLessonId_LessonStudentStudentId",
                table: "QuizResult");

            migrationBuilder.DropForeignKey(
                name: "FK_QuizResult_students_StudentId",
                table: "QuizResult");

            migrationBuilder.DropPrimaryKey(
                name: "PK_QuizResult",
                table: "QuizResult");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Note",
                table: "Note");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LessonVideo",
                table: "LessonVideo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LessonQuiz",
                table: "LessonQuiz");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LessonMaterial",
                table: "LessonMaterial");

            migrationBuilder.RenameTable(
                name: "QuizResult",
                newName: "quizResults");

            migrationBuilder.RenameTable(
                name: "Note",
                newName: "notes");

            migrationBuilder.RenameTable(
                name: "LessonVideo",
                newName: "lessonsVideo");

            migrationBuilder.RenameTable(
                name: "LessonQuiz",
                newName: "lessonQuizzes");

            migrationBuilder.RenameTable(
                name: "LessonMaterial",
                newName: "lessonsMaterial");

            migrationBuilder.RenameIndex(
                name: "IX_QuizResult_StudentId",
                table: "quizResults",
                newName: "IX_quizResults_StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_QuizResult_QuizId",
                table: "quizResults",
                newName: "IX_quizResults_QuizId");

            migrationBuilder.RenameIndex(
                name: "IX_QuizResult_LessonStudentLessonId_LessonStudentStudentId",
                table: "quizResults",
                newName: "IX_quizResults_LessonStudentLessonId_LessonStudentStudentId");

            migrationBuilder.RenameIndex(
                name: "IX_Note_AppUserId",
                table: "notes",
                newName: "IX_notes_AppUserId");

            migrationBuilder.RenameIndex(
                name: "IX_LessonVideo_LessonId",
                table: "lessonsVideo",
                newName: "IX_lessonsVideo_LessonId");

            migrationBuilder.RenameIndex(
                name: "IX_LessonQuiz_LessonId",
                table: "lessonQuizzes",
                newName: "IX_lessonQuizzes_LessonId");

            migrationBuilder.RenameIndex(
                name: "IX_LessonMaterial_LessonId",
                table: "lessonsMaterial",
                newName: "IX_lessonsMaterial_LessonId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_quizResults",
                table: "quizResults",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_notes",
                table: "notes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_lessonsVideo",
                table: "lessonsVideo",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_lessonQuizzes",
                table: "lessonQuizzes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_lessonsMaterial",
                table: "lessonsMaterial",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "quizQuestions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QuestionType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CorrectAnswer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LessonQuizId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_quizQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_quizQuestions_lessonQuizzes_LessonQuizId",
                        column: x => x.LessonQuizId,
                        principalTable: "lessonQuizzes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "requestToRegister",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Age = table.Column<int>(type: "int", nullable: false),
                    IsParent = table.Column<bool>(type: "bit", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExistedCourses = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChoosenCourse = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChildName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChildAge = table.Column<int>(type: "int", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_requestToRegister", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "quizOptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsCorrect = table.Column<bool>(type: "bit", nullable: false),
                    QuizQuestionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_quizOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_quizOptions_quizQuestions_QuizQuestionId",
                        column: x => x.QuizQuestionId,
                        principalTable: "quizQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_quizOptions_QuizQuestionId",
                table: "quizOptions",
                column: "QuizQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_quizQuestions_LessonQuizId",
                table: "quizQuestions",
                column: "LessonQuizId");

            migrationBuilder.AddForeignKey(
                name: "FK_lessonQuizzes_lessons_LessonId",
                table: "lessonQuizzes",
                column: "LessonId",
                principalTable: "lessons",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_lessonsMaterial_lessons_LessonId",
                table: "lessonsMaterial",
                column: "LessonId",
                principalTable: "lessons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_lessonsVideo_lessons_LessonId",
                table: "lessonsVideo",
                column: "LessonId",
                principalTable: "lessons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_notes_AspNetUsers_AppUserId",
                table: "notes",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_quizResults_lessonQuizzes_QuizId",
                table: "quizResults",
                column: "QuizId",
                principalTable: "lessonQuizzes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_quizResults_lessonsStudents_LessonStudentLessonId_LessonStudentStudentId",
                table: "quizResults",
                columns: new[] { "LessonStudentLessonId", "LessonStudentStudentId" },
                principalTable: "lessonsStudents",
                principalColumns: new[] { "LessonId", "StudentId" });

            migrationBuilder.AddForeignKey(
                name: "FK_quizResults_students_StudentId",
                table: "quizResults",
                column: "StudentId",
                principalTable: "students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_lessonQuizzes_lessons_LessonId",
                table: "lessonQuizzes");

            migrationBuilder.DropForeignKey(
                name: "FK_lessonsMaterial_lessons_LessonId",
                table: "lessonsMaterial");

            migrationBuilder.DropForeignKey(
                name: "FK_lessonsVideo_lessons_LessonId",
                table: "lessonsVideo");

            migrationBuilder.DropForeignKey(
                name: "FK_notes_AspNetUsers_AppUserId",
                table: "notes");

            migrationBuilder.DropForeignKey(
                name: "FK_quizResults_lessonQuizzes_QuizId",
                table: "quizResults");

            migrationBuilder.DropForeignKey(
                name: "FK_quizResults_lessonsStudents_LessonStudentLessonId_LessonStudentStudentId",
                table: "quizResults");

            migrationBuilder.DropForeignKey(
                name: "FK_quizResults_students_StudentId",
                table: "quizResults");

            migrationBuilder.DropTable(
                name: "quizOptions");

            migrationBuilder.DropTable(
                name: "requestToRegister");

            migrationBuilder.DropTable(
                name: "quizQuestions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_quizResults",
                table: "quizResults");

            migrationBuilder.DropPrimaryKey(
                name: "PK_notes",
                table: "notes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_lessonsVideo",
                table: "lessonsVideo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_lessonsMaterial",
                table: "lessonsMaterial");

            migrationBuilder.DropPrimaryKey(
                name: "PK_lessonQuizzes",
                table: "lessonQuizzes");

            migrationBuilder.RenameTable(
                name: "quizResults",
                newName: "QuizResult");

            migrationBuilder.RenameTable(
                name: "notes",
                newName: "Note");

            migrationBuilder.RenameTable(
                name: "lessonsVideo",
                newName: "LessonVideo");

            migrationBuilder.RenameTable(
                name: "lessonsMaterial",
                newName: "LessonMaterial");

            migrationBuilder.RenameTable(
                name: "lessonQuizzes",
                newName: "LessonQuiz");

            migrationBuilder.RenameIndex(
                name: "IX_quizResults_StudentId",
                table: "QuizResult",
                newName: "IX_QuizResult_StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_quizResults_QuizId",
                table: "QuizResult",
                newName: "IX_QuizResult_QuizId");

            migrationBuilder.RenameIndex(
                name: "IX_quizResults_LessonStudentLessonId_LessonStudentStudentId",
                table: "QuizResult",
                newName: "IX_QuizResult_LessonStudentLessonId_LessonStudentStudentId");

            migrationBuilder.RenameIndex(
                name: "IX_notes_AppUserId",
                table: "Note",
                newName: "IX_Note_AppUserId");

            migrationBuilder.RenameIndex(
                name: "IX_lessonsVideo_LessonId",
                table: "LessonVideo",
                newName: "IX_LessonVideo_LessonId");

            migrationBuilder.RenameIndex(
                name: "IX_lessonsMaterial_LessonId",
                table: "LessonMaterial",
                newName: "IX_LessonMaterial_LessonId");

            migrationBuilder.RenameIndex(
                name: "IX_lessonQuizzes_LessonId",
                table: "LessonQuiz",
                newName: "IX_LessonQuiz_LessonId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_QuizResult",
                table: "QuizResult",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Note",
                table: "Note",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LessonVideo",
                table: "LessonVideo",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LessonMaterial",
                table: "LessonMaterial",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LessonQuiz",
                table: "LessonQuiz",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LessonMaterial_lessons_LessonId",
                table: "LessonMaterial",
                column: "LessonId",
                principalTable: "lessons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LessonQuiz_lessons_LessonId",
                table: "LessonQuiz",
                column: "LessonId",
                principalTable: "lessons",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LessonVideo_lessons_LessonId",
                table: "LessonVideo",
                column: "LessonId",
                principalTable: "lessons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Note_AspNetUsers_AppUserId",
                table: "Note",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_QuizResult_LessonQuiz_QuizId",
                table: "QuizResult",
                column: "QuizId",
                principalTable: "LessonQuiz",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuizResult_lessonsStudents_LessonStudentLessonId_LessonStudentStudentId",
                table: "QuizResult",
                columns: new[] { "LessonStudentLessonId", "LessonStudentStudentId" },
                principalTable: "lessonsStudents",
                principalColumns: new[] { "LessonId", "StudentId" });

            migrationBuilder.AddForeignKey(
                name: "FK_QuizResult_students_StudentId",
                table: "QuizResult",
                column: "StudentId",
                principalTable: "students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
