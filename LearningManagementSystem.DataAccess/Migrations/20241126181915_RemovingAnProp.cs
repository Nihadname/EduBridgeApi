using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearningManagementSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RemovingAnProp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExistedCourses",
                table: "requestToRegister");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExistedCourses",
                table: "requestToRegister",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
