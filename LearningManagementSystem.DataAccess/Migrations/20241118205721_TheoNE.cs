using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearningManagementSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class TheoNE : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LessonMaterial_lessons_LessonId",
                table: "LessonMaterial");

            migrationBuilder.AlterColumn<Guid>(
                name: "LessonId",
                table: "LessonMaterial",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_LessonMaterial_lessons_LessonId",
                table: "LessonMaterial",
                column: "LessonId",
                principalTable: "lessons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LessonMaterial_lessons_LessonId",
                table: "LessonMaterial");

            migrationBuilder.AlterColumn<Guid>(
                name: "LessonId",
                table: "LessonMaterial",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_LessonMaterial_lessons_LessonId",
                table: "LessonMaterial",
                column: "LessonId",
                principalTable: "lessons",
                principalColumn: "Id");
        }
    }
}
