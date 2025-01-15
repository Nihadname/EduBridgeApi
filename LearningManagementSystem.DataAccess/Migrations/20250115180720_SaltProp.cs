using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearningManagementSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class SaltProp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Salt",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Salt",
                table: "AspNetUsers");
        }
    }
}
