using Microsoft.EntityFrameworkCore.Migrations;

namespace CSharpExam.Migrations
{
    public partial class EditMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "proficiency",
                table: "Hobbies");

            migrationBuilder.AddColumn<string>(
                name: "Proficiency",
                table: "Associations",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Proficiency",
                table: "Associations");

            migrationBuilder.AddColumn<string>(
                name: "proficiency",
                table: "Hobbies",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);
        }
    }
}
