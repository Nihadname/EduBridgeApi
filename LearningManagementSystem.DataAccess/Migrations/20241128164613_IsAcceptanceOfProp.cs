﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearningManagementSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class IsAcceptanceOfProp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAccepted",
                table: "requestToRegister",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAccepted",
                table: "requestToRegister");
        }
    }
}
