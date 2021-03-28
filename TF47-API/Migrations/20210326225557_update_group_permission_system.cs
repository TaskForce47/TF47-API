using Microsoft.EntityFrameworkCore.Migrations;

namespace TF47_API.Migrations
{
    public partial class update_group_permission_system : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "can_ban_permanent",
                table: "ServiceGroupPermissions");

            migrationBuilder.DropColumn(
                name: "can_ban_users",
                table: "ServiceGroupPermissions");

            migrationBuilder.DropColumn(
                name: "can_create_servers",
                table: "ServiceGroupPermissions");

            migrationBuilder.DropColumn(
                name: "can_delete_users",
                table: "ServiceGroupPermissions");

            migrationBuilder.DropColumn(
                name: "can_edit_groups",
                table: "ServiceGroupPermissions");

            migrationBuilder.DropColumn(
                name: "can_edit_servers",
                table: "ServiceGroupPermissions");

            migrationBuilder.DropColumn(
                name: "can_edit_users",
                table: "ServiceGroupPermissions");

            migrationBuilder.DropColumn(
                name: "can_use_servers",
                table: "ServiceGroupPermissions");

            migrationBuilder.AddColumn<string>(
                name: "permissions_discord",
                table: "ServiceGroupPermissions",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "permissions_gadget",
                table: "ServiceGroupPermissions",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "permissions_teamspeak",
                table: "ServiceGroupPermissions",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "player_name",
                table: "GameServerPlayers",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "permissions_discord",
                table: "ServiceGroupPermissions");

            migrationBuilder.DropColumn(
                name: "permissions_gadget",
                table: "ServiceGroupPermissions");

            migrationBuilder.DropColumn(
                name: "permissions_teamspeak",
                table: "ServiceGroupPermissions");

            migrationBuilder.DropColumn(
                name: "player_name",
                table: "GameServerPlayers");

            migrationBuilder.AddColumn<bool>(
                name: "can_ban_permanent",
                table: "ServiceGroupPermissions",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "can_ban_users",
                table: "ServiceGroupPermissions",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "can_create_servers",
                table: "ServiceGroupPermissions",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "can_delete_users",
                table: "ServiceGroupPermissions",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "can_edit_groups",
                table: "ServiceGroupPermissions",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "can_edit_servers",
                table: "ServiceGroupPermissions",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "can_edit_users",
                table: "ServiceGroupPermissions",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "can_use_servers",
                table: "ServiceGroupPermissions",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
