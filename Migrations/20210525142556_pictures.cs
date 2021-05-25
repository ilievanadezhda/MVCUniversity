using Microsoft.EntityFrameworkCore.Migrations;

namespace MVCUniversity.Migrations
{
    public partial class pictures : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "profilePicture",
                table: "Teacher",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "profilePicture",
                table: "Teacher");
        }
    }
}
