using Microsoft.EntityFrameworkCore.Migrations;

namespace Tetas.Infraestructure.Migrations
{
    public partial class AddingBanned : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Banned",
                table: "GroupMembers",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Banned",
                table: "GroupMembers");
        }
    }
}
