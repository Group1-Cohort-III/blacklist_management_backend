using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlackGuardApp.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class updatedmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "BlacklistCriterias");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "BlacklistCriterias");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "BlacklistCriterias");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "BlacklistCriterias");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "BlacklistCriterias",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "BlacklistCriterias",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "BlacklistCriterias",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "BlacklistCriterias",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
