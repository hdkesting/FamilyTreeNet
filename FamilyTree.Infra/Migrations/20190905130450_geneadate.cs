using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FamilyTree.Infra.Migrations
{
    public partial class geneadate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder is null)
            {
                throw new ArgumentNullException(nameof(migrationBuilder));
            }

            migrationBuilder.DropColumn(
                name: "BirthDate",
                table: "Individuals");

            migrationBuilder.DropColumn(
                name: "DeathDate",
                table: "Individuals");

            migrationBuilder.DropColumn(
                name: "DivorceDate",
                table: "Families");

            migrationBuilder.DropColumn(
                name: "MarriageDate",
                table: "Families");

            migrationBuilder.AddColumn<int>(
                name: "BirthDateInt",
                table: "Individuals",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeathDateInt",
                table: "Individuals",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DivorceDateInt",
                table: "Families",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MarriageDateInt",
                table: "Families",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder is null)
            {
                throw new ArgumentNullException(nameof(migrationBuilder));
            }

            migrationBuilder.DropColumn(
                name: "BirthDateInt",
                table: "Individuals");

            migrationBuilder.DropColumn(
                name: "DeathDateInt",
                table: "Individuals");

            migrationBuilder.DropColumn(
                name: "DivorceDateInt",
                table: "Families");

            migrationBuilder.DropColumn(
                name: "MarriageDateInt",
                table: "Families");

            migrationBuilder.AddColumn<DateTime>(
                name: "BirthDate",
                table: "Individuals",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeathDate",
                table: "Individuals",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DivorceDate",
                table: "Families",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "MarriageDate",
                table: "Families",
                nullable: true);
        }
    }
}
