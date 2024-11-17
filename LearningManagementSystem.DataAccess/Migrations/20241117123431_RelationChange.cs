using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearningManagementSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RelationChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_students_ParentId",
                table: "students");

            migrationBuilder.CreateIndex(
                name: "IX_students_ParentId",
                table: "students",
                column: "ParentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_students_ParentId",
                table: "students");

            migrationBuilder.CreateIndex(
                name: "IX_students_ParentId",
                table: "students",
                column: "ParentId",
                unique: true);
        }
    }
}
