using Microsoft.EntityFrameworkCore.Migrations;

namespace Tetas.Infraestructure.Migrations
{
    public partial class AddingNickName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NickName",
                table: "Users",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NickName",
                table: "Users");
        }
    }
}
