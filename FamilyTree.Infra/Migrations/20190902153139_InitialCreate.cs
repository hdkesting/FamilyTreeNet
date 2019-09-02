using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FamilyTree.Infra.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Families",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false),
                    MarriageDate = table.Column<DateTime>(nullable: true),
                    MarriagePlace = table.Column<string>(nullable: true),
                    DivorceDate = table.Column<DateTime>(nullable: true),
                    DivorcePlace = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Families", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Individuals",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false),
                    Firstnames = table.Column<string>(nullable: true),
                    Lastname = table.Column<string>(nullable: true),
                    Sex = table.Column<string>(nullable: false),
                    BirthDate = table.Column<DateTime>(nullable: true),
                    BirthPlace = table.Column<string>(nullable: true),
                    DeathDate = table.Column<DateTime>(nullable: true),
                    DeathPlace = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Individuals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChildRelation",
                columns: table => new
                {
                    ChildId = table.Column<long>(nullable: false),
                    ChildFamilyId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChildRelation", x => new { x.ChildId, x.ChildFamilyId });
                    table.ForeignKey(
                        name: "FK_ChildRelation_Families_ChildFamilyId",
                        column: x => x.ChildFamilyId,
                        principalTable: "Families",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChildRelation_Individuals_ChildId",
                        column: x => x.ChildId,
                        principalTable: "Individuals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SpouseRelation",
                columns: table => new
                {
                    SpouseId = table.Column<long>(nullable: false),
                    SpouseFamilyId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpouseRelation", x => new { x.SpouseId, x.SpouseFamilyId });
                    table.ForeignKey(
                        name: "FK_SpouseRelation_Families_SpouseFamilyId",
                        column: x => x.SpouseFamilyId,
                        principalTable: "Families",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SpouseRelation_Individuals_SpouseId",
                        column: x => x.SpouseId,
                        principalTable: "Individuals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChildRelation_ChildFamilyId",
                table: "ChildRelation",
                column: "ChildFamilyId");

            migrationBuilder.CreateIndex(
                name: "IX_SpouseRelation_SpouseFamilyId",
                table: "SpouseRelation",
                column: "SpouseFamilyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChildRelation");

            migrationBuilder.DropTable(
                name: "SpouseRelation");

            migrationBuilder.DropTable(
                name: "Families");

            migrationBuilder.DropTable(
                name: "Individuals");
        }
    }
}
