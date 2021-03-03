using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace TF47_Backend.Migrations
{
    public partial class add_user_db : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    banned = table.Column<bool>(type: "boolean", nullable: false),
                    mail = table.Column<string>(type: "text", nullable: true),
                    username = table.Column<string>(type: "text", nullable: true),
                    password = table.Column<string>(type: "text", nullable: true),
                    profile_picture = table.Column<string>(type: "text", nullable: true),
                    profile_background = table.Column<string>(type: "text", nullable: true),
                    first_time_seen = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    last_time_seen = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.user_id);
                });

            migrationBuilder.CreateTable(
                name: "groups",
                columns: table => new
                {
                    group_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    text_color = table.Column<string>(type: "text", nullable: true),
                    background_color = table.Column<string>(type: "text", nullable: true),
                    is_visible = table.Column<bool>(type: "boolean", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_groups", x => x.group_id);
                    table.ForeignKey(
                        name: "fk_groups_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "group_permissions",
                columns: table => new
                {
                    group_permission_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    group_id = table.Column<long>(type: "bigint", nullable: false),
                    can_edit_groups = table.Column<bool>(type: "boolean", nullable: false),
                    can_ban_users = table.Column<bool>(type: "boolean", nullable: false),
                    can_perma_ban = table.Column<bool>(type: "boolean", nullable: false),
                    can_edit_users = table.Column<bool>(type: "boolean", nullable: false),
                    can_delete_users = table.Column<bool>(type: "boolean", nullable: false),
                    can_edit_servers = table.Column<bool>(type: "boolean", nullable: false),
                    can_create_servers = table.Column<bool>(type: "boolean", nullable: false),
                    can_use_servers = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_group_permissions", x => x.group_permission_id);
                    table.ForeignKey(
                        name: "fk_group_permissions_groups_group_id",
                        column: x => x.group_id,
                        principalTable: "groups",
                        principalColumn: "group_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_group_permissions_group_id",
                table: "group_permissions",
                column: "group_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_groups_user_id",
                table: "groups",
                column: "user_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "group_permissions");

            migrationBuilder.DropTable(
                name: "groups");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
