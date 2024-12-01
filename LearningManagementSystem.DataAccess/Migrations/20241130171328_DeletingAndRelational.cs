using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearningManagementSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class DeletingAndRelational : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_notes_AspNetUsers_AppUserId",
                table: "notes");

            migrationBuilder.DropForeignKey(
                name: "FK_parents_AspNetUsers_AppUserId",
                table: "parents");

            migrationBuilder.DropForeignKey(
                name: "FK_students_AspNetUsers_AppUserId",
                table: "students");

            migrationBuilder.DropForeignKey(
                name: "FK_teachers_AspNetUsers_AppUserId",
                table: "teachers");

            migrationBuilder.AddForeignKey(
                name: "FK_notes_AspNetUsers_AppUserId",
                table: "notes",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_parents_AspNetUsers_AppUserId",
                table: "parents",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_students_AspNetUsers_AppUserId",
                table: "students",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_teachers_AspNetUsers_AppUserId",
                table: "teachers",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_notes_AspNetUsers_AppUserId",
                table: "notes");

            migrationBuilder.DropForeignKey(
                name: "FK_parents_AspNetUsers_AppUserId",
                table: "parents");

            migrationBuilder.DropForeignKey(
                name: "FK_students_AspNetUsers_AppUserId",
                table: "students");

            migrationBuilder.DropForeignKey(
                name: "FK_teachers_AspNetUsers_AppUserId",
                table: "teachers");

            migrationBuilder.AddForeignKey(
                name: "FK_notes_AspNetUsers_AppUserId",
                table: "notes",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_parents_AspNetUsers_AppUserId",
                table: "parents",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_students_AspNetUsers_AppUserId",
                table: "students",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_teachers_AspNetUsers_AppUserId",
                table: "teachers",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
