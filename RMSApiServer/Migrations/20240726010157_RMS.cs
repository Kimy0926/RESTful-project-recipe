using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RMSApiServer.Migrations
{
    public partial class RMS : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Recipe",
                columns: table => new
                {
                    RecipeId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SiteId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RecipeName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recipe", x => new { x.RecipeId, x.SiteId });
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Equipment",
                columns: table => new
                {
                    EquipmentId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SiteId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EquipmentName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    RecipeId = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RecipeSiteId = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipment", x => new { x.EquipmentId, x.SiteId });
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "RecipeParameter",
                columns: table => new
                {
                    RecipeId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RecipeParamId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SiteId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RecipeParamName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    USL = table.Column<float>(type: "float", nullable: false),
                    LSL = table.Column<float>(type: "float", nullable: false),
                    Target = table.Column<float>(type: "float", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeParameter", x => new { x.RecipeId, x.RecipeParamId, x.SiteId });
                    table.ForeignKey(
                        name: "FK_RecipeParameter_Recipe_RecipeId_SiteId",
                        columns: x => new { x.RecipeId, x.SiteId },
                        principalTable: "Recipe",
                        principalColumns: new[] { "RecipeId", "SiteId" },
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EquipmentRecipeMap",
                columns: table => new
                {
                    RecipeId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EquipmentId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SiteId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentRecipeMap", x => new { x.EquipmentId, x.RecipeId, x.SiteId });
                    table.ForeignKey(
                        name: "FK_EquipmentRecipeMap_Equipment_EquipmentId_SiteId",
                        columns: x => new { x.EquipmentId, x.SiteId },
                        principalTable: "Equipment",
                        principalColumns: new[] { "EquipmentId", "SiteId" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EquipmentRecipeMap_Recipe_RecipeId_SiteId",
                        columns: x => new { x.RecipeId, x.SiteId },
                        principalTable: "Recipe",
                        principalColumns: new[] { "RecipeId", "SiteId" },
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Equipment_RecipeId_RecipeSiteId",
                table: "Equipment",
                columns: new[] { "RecipeId", "RecipeSiteId" });

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentRecipeMap_EquipmentId_SiteId",
                table: "EquipmentRecipeMap",
                columns: new[] { "EquipmentId", "SiteId" });

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentRecipeMap_RecipeId_SiteId",
                table: "EquipmentRecipeMap",
                columns: new[] { "RecipeId", "SiteId" });

            migrationBuilder.CreateIndex(
                name: "IX_RecipeParameter_RecipeId_SiteId",
                table: "RecipeParameter",
                columns: new[] { "RecipeId", "SiteId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EquipmentRecipeMap");

            migrationBuilder.DropTable(
                name: "RecipeParameter");

            migrationBuilder.DropTable(
                name: "Equipment");

            migrationBuilder.DropTable(
                name: "Recipe");
        }
    }
}
