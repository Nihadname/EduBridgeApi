using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearningManagementSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class NewPropsToAuth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiredDate",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsEmailVerificationCodeValid",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "VerificationCode",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpiredDate",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsEmailVerificationCodeValid",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "VerificationCode",
                table: "AspNetUsers");
        }
    }
}
