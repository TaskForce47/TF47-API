using Microsoft.EntityFrameworkCore.Migrations;

namespace TF47_Backend.Migrations
{
    public partial class added_discord_to_user_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "mail",
                table: "ServiceUsers",
                newName: "email");

            migrationBuilder.AddColumn<bool>(
                name: "allow_emails",
                table: "ServiceUsers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "discord_id",
                table: "ServiceUsers",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "allow_emails",
                table: "ServiceUsers");

            migrationBuilder.DropColumn(
                name: "discord_id",
                table: "ServiceUsers");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "ServiceUsers",
                newName: "mail");
        }
    }
}
