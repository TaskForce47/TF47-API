using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace TF47_API.Migrations
{
    public partial class added_issues_and_reorganisation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "gameserver_whitelistings");

            migrationBuilder.DropTable(
                name: "service_userhasgroups");

            migrationBuilder.CreateTable(
                name: "gameserver_playerwhitelistings",
                columns: table => new
                {
                    player_whitelistings_whitelist_id = table.Column<long>(type: "bigint", nullable: false),
                    players_player_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_gameserver_playerwhitelistings", x => new { x.player_whitelistings_whitelist_id, x.players_player_id });
                    table.ForeignKey(
                        name: "fk_gameserver_playerwhitelistings_players_players_player_id",
                        column: x => x.players_player_id,
                        principalTable: "gameserver_players",
                        principalColumn: "player_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_gameserver_playerwhitelistings_whitelist_player_whitelistin",
                        column: x => x.player_whitelistings_whitelist_id,
                        principalTable: "gameserver_whitelists",
                        principalColumn: "whitelist_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "service_issuegroups",
                columns: table => new
                {
                    issue_group_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    group_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    group_description = table.Column<string>(type: "text", nullable: true),
                    time_group_created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_service_issuegroups", x => x.issue_group_id);
                });

            migrationBuilder.CreateTable(
                name: "service_usergroups",
                columns: table => new
                {
                    groups_group_id = table.Column<long>(type: "bigint", nullable: false),
                    users_user_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_service_usergroups", x => new { x.groups_group_id, x.users_user_id });
                    table.ForeignKey(
                        name: "fk_service_usergroups_groups_groups_group_id",
                        column: x => x.groups_group_id,
                        principalTable: "service_groups",
                        principalColumn: "group_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_service_usergroups_users_users_user_id",
                        column: x => x.users_user_id,
                        principalTable: "service_users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "service_issues",
                columns: table => new
                {
                    issue_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    is_closed = table.Column<bool>(type: "boolean", nullable: false),
                    tags = table.Column<string[]>(type: "text[]", nullable: true),
                    issue_creator_user_id = table.Column<Guid>(type: "uuid", nullable: true),
                    time_created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    time_last_updated = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    issue_group_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_service_issues", x => x.issue_id);
                    table.ForeignKey(
                        name: "fk_service_issues_service_issuegroups_issue_group_id",
                        column: x => x.issue_group_id,
                        principalTable: "service_issuegroups",
                        principalColumn: "issue_group_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_service_issues_service_users_issue_creator_user_id",
                        column: x => x.issue_creator_user_id,
                        principalTable: "service_users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "service_issueitems",
                columns: table => new
                {
                    issue_item_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    author_user_id = table.Column<Guid>(type: "uuid", nullable: true),
                    message = table.Column<string>(type: "text", nullable: true),
                    time_created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    is_edited = table.Column<bool>(type: "boolean", nullable: false),
                    time_last_edited = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    issue_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_service_issueitems", x => x.issue_item_id);
                    table.ForeignKey(
                        name: "fk_service_issueitems_service_issues_issue_id",
                        column: x => x.issue_id,
                        principalTable: "service_issues",
                        principalColumn: "issue_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_service_issueitems_service_users_author_user_id",
                        column: x => x.author_user_id,
                        principalTable: "service_users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_gameserver_playerwhitelistings_players_player_id",
                table: "gameserver_playerwhitelistings",
                column: "players_player_id");

            migrationBuilder.CreateIndex(
                name: "ix_service_issuegroups_group_name",
                table: "service_issuegroups",
                column: "group_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_service_issueitems_author_user_id",
                table: "service_issueitems",
                column: "author_user_id");

            migrationBuilder.CreateIndex(
                name: "ix_service_issueitems_issue_id",
                table: "service_issueitems",
                column: "issue_id");

            migrationBuilder.CreateIndex(
                name: "ix_service_issues_issue_creator_user_id",
                table: "service_issues",
                column: "issue_creator_user_id");

            migrationBuilder.CreateIndex(
                name: "ix_service_issues_issue_group_id",
                table: "service_issues",
                column: "issue_group_id");

            migrationBuilder.CreateIndex(
                name: "ix_service_usergroups_users_user_id",
                table: "service_usergroups",
                column: "users_user_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "gameserver_playerwhitelistings");

            migrationBuilder.DropTable(
                name: "service_issueitems");

            migrationBuilder.DropTable(
                name: "service_usergroups");

            migrationBuilder.DropTable(
                name: "service_issues");

            migrationBuilder.DropTable(
                name: "service_issuegroups");

            migrationBuilder.CreateTable(
                name: "gameserver_whitelistings",
                columns: table => new
                {
                    whitelisting_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    player_id = table.Column<long>(type: "bigint", nullable: true),
                    whitelist_id = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_gameserver_whitelistings", x => x.whitelisting_id);
                    table.ForeignKey(
                        name: "fk_gameserver_whitelistings_gameserver_players_player_id",
                        column: x => x.player_id,
                        principalTable: "gameserver_players",
                        principalColumn: "player_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_gameserver_whitelistings_gameserver_whitelists_whitelist_id",
                        column: x => x.whitelist_id,
                        principalTable: "gameserver_whitelists",
                        principalColumn: "whitelist_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "service_userhasgroups",
                columns: table => new
                {
                    user_has_group_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    group_id = table.Column<long>(type: "bigint", nullable: true),
                    time_added_to_group = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_service_userhasgroups", x => x.user_has_group_id);
                    table.ForeignKey(
                        name: "fk_service_userhasgroups_service_groups_group_id",
                        column: x => x.group_id,
                        principalTable: "service_groups",
                        principalColumn: "group_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_service_userhasgroups_service_users_user_id",
                        column: x => x.user_id,
                        principalTable: "service_users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_gameserver_whitelistings_player_id",
                table: "gameserver_whitelistings",
                column: "player_id");

            migrationBuilder.CreateIndex(
                name: "ix_gameserver_whitelistings_whitelist_id",
                table: "gameserver_whitelistings",
                column: "whitelist_id");

            migrationBuilder.CreateIndex(
                name: "ix_service_userhasgroups_group_id",
                table: "service_userhasgroups",
                column: "group_id");

            migrationBuilder.CreateIndex(
                name: "ix_service_userhasgroups_user_id",
                table: "service_userhasgroups",
                column: "user_id");
        }
    }
}
