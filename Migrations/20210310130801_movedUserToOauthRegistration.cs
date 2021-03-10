using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace TF47_Backend.Migrations
{
    public partial class movedUserToOauthRegistration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "service_password_resets");

            migrationBuilder.DropColumn(
                name: "is_connected_steam",
                table: "service_users");

            migrationBuilder.DropColumn(
                name: "password",
                table: "service_users");

            migrationBuilder.RenameColumn(
                name: "profile_background",
                table: "service_users",
                newName: "profile_url");

            migrationBuilder.AlterColumn<string>(
                name: "steam_id",
                table: "service_users",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "mail",
                table: "service_users",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "country_code",
                table: "service_users",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_service_users_steam_id",
                table: "service_users",
                column: "steam_id",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_service_users_steam_id",
                table: "service_users");

            migrationBuilder.DropColumn(
                name: "country_code",
                table: "service_users");

            migrationBuilder.RenameColumn(
                name: "profile_url",
                table: "service_users",
                newName: "profile_background");

            migrationBuilder.AlterColumn<string>(
                name: "steam_id",
                table: "service_users",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "mail",
                table: "service_users",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_connected_steam",
                table: "service_users",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "password",
                table: "service_users",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "service_password_resets",
                columns: table => new
                {
                    password_reset_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    reset_token = table.Column<string>(type: "text", nullable: true),
                    time_password_reset_generated = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_service_password_resets", x => x.password_reset_id);
                    table.ForeignKey(
                        name: "fk_service_password_resets_service_users_user_id",
                        column: x => x.user_id,
                        principalTable: "service_users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_service_password_resets_user_id",
                table: "service_password_resets",
                column: "user_id");
        }
    }
}
