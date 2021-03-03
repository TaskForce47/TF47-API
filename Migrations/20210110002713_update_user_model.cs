using Microsoft.EntityFrameworkCore.Migrations;

namespace TF47_Backend.Migrations
{
    public partial class update_user_model : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_connected_steam",
                table: "users",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "steam_id",
                table: "users",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_connected_steam",
                table: "users");

            migrationBuilder.DropColumn(
                name: "steam_id",
                table: "users");
        }
    }
}
