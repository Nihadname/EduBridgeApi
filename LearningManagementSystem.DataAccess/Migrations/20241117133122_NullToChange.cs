using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearningManagementSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class NullToChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_students_parents_ParentId",
                table: "students");

            migrationBuilder.AlterColumn<Guid>(
                name: "ParentId",
                table: "students",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_students_parents_ParentId",
                table: "students",
                column: "ParentId",
                principalTable: "parents",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_students_parents_ParentId",
                table: "students");

            migrationBuilder.AlterColumn<Guid>(
                name: "ParentId",
                table: "students",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_students_parents_ParentId",
                table: "students",
                column: "ParentId",
                principalTable: "parents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
