using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlackGuardApp.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class blacklistmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlacklistHistories_BlacklistItems_BlackListedProductId",
                table: "BlacklistHistories");

            migrationBuilder.DropTable(
                name: "BlacklistItems");

            migrationBuilder.DropIndex(
                name: "IX_BlacklistHistories_BlackListedProductId",
                table: "BlacklistHistories");

            migrationBuilder.DropColumn(
                name: "BlackListProductId",
                table: "BlacklistHistories");

            migrationBuilder.DropColumn(
                name: "BlackListedProductId",
                table: "BlacklistHistories");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "BlacklistHistories");

            migrationBuilder.RenameColumn(
                name: "CriteriaName",
                table: "BlacklistCriterias",
                newName: "CategoryName");

            migrationBuilder.RenameColumn(
                name: "CriteriaDescription",
                table: "BlacklistCriterias",
                newName: "CategoryDescription");

            migrationBuilder.AddColumn<string>(
                name: "BlackListId",
                table: "BlacklistHistories",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "BlackLists",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProductId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BlacklistCriteriaId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlackLists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BlackLists_BlacklistCriterias_BlacklistCriteriaId",
                        column: x => x.BlacklistCriteriaId,
                        principalTable: "BlacklistCriterias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BlackLists_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BlacklistHistories_BlackListId",
                table: "BlacklistHistories",
                column: "BlackListId");

            migrationBuilder.CreateIndex(
                name: "IX_BlackLists_BlacklistCriteriaId",
                table: "BlackLists",
                column: "BlacklistCriteriaId");

            migrationBuilder.CreateIndex(
                name: "IX_BlackLists_ProductId",
                table: "BlackLists",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_BlacklistHistories_BlackLists_BlackListId",
                table: "BlacklistHistories",
                column: "BlackListId",
                principalTable: "BlackLists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlacklistHistories_BlackLists_BlackListId",
                table: "BlacklistHistories");

            migrationBuilder.DropTable(
                name: "BlackLists");

            migrationBuilder.DropIndex(
                name: "IX_BlacklistHistories_BlackListId",
                table: "BlacklistHistories");

            migrationBuilder.DropColumn(
                name: "BlackListId",
                table: "BlacklistHistories");

            migrationBuilder.RenameColumn(
                name: "CategoryName",
                table: "BlacklistCriterias",
                newName: "CriteriaName");

            migrationBuilder.RenameColumn(
                name: "CategoryDescription",
                table: "BlacklistCriterias",
                newName: "CriteriaDescription");

            migrationBuilder.AddColumn<string>(
                name: "BlackListProductId",
                table: "BlacklistHistories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BlackListedProductId",
                table: "BlacklistHistories",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "BlacklistHistories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "BlacklistItems",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AppUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    BlacklistCriteriaId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProductId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlacklistItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BlacklistItems_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BlacklistItems_BlacklistCriterias_BlacklistCriteriaId",
                        column: x => x.BlacklistCriteriaId,
                        principalTable: "BlacklistCriterias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BlacklistItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BlacklistHistories_BlackListedProductId",
                table: "BlacklistHistories",
                column: "BlackListedProductId");

            migrationBuilder.CreateIndex(
                name: "IX_BlacklistItems_AppUserId",
                table: "BlacklistItems",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_BlacklistItems_BlacklistCriteriaId",
                table: "BlacklistItems",
                column: "BlacklistCriteriaId");

            migrationBuilder.CreateIndex(
                name: "IX_BlacklistItems_ProductId",
                table: "BlacklistItems",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_BlacklistHistories_BlacklistItems_BlackListedProductId",
                table: "BlacklistHistories",
                column: "BlackListedProductId",
                principalTable: "BlacklistItems",
                principalColumn: "Id");
        }
    }
}
