using Microsoft.EntityFrameworkCore.Migrations;

namespace Tetas.Infraestructure.Migrations
{
    public partial class AddingApplied : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Applied",
                table: "GroupMembers",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Applied",
                table: "GroupMembers");
        }
    }
}
