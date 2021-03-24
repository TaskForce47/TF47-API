using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using NpgsqlTypes;

namespace TF47_Backend.Migrations
{
    public partial class updated_all_tables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_gameserver_chats_gameserver_players_player_id",
                table: "gameserver_chats");

            migrationBuilder.DropForeignKey(
                name: "fk_gameserver_chats_gameserver_sessions_session_id",
                table: "gameserver_chats");

            migrationBuilder.DropForeignKey(
                name: "fk_gameserver_kills_gameserver_players_killer_player_id",
                table: "gameserver_kills");

            migrationBuilder.DropForeignKey(
                name: "fk_gameserver_kills_gameserver_players_victim_player_id",
                table: "gameserver_kills");

            migrationBuilder.DropForeignKey(
                name: "fk_gameserver_kills_gameserver_sessions_session_id",
                table: "gameserver_kills");

            migrationBuilder.DropForeignKey(
                name: "fk_gameserver_missions_gameserver_campaigns_campaign_id",
                table: "gameserver_missions");

            migrationBuilder.DropForeignKey(
                name: "fk_gameserver_playerwhitelistings_players_players_player_id",
                table: "gameserver_playerwhitelistings");

            migrationBuilder.DropForeignKey(
                name: "fk_gameserver_playerwhitelistings_whitelist_player_whitelistin",
                table: "gameserver_playerwhitelistings");

            migrationBuilder.DropForeignKey(
                name: "fk_gameserver_playtimes_gameserver_players_player_id",
                table: "gameserver_playtimes");

            migrationBuilder.DropForeignKey(
                name: "fk_gameserver_playtimes_gameserver_sessions_session_id",
                table: "gameserver_playtimes");

            migrationBuilder.DropForeignKey(
                name: "fk_gameserver_sessions_missions_mission_id",
                table: "gameserver_sessions");

            migrationBuilder.DropForeignKey(
                name: "fk_service_grouppermissions_service_groups_group_id",
                table: "service_grouppermissions");

            migrationBuilder.DropForeignKey(
                name: "fk_service_issue_has_tags_issue_tags_issue_tags_issue_tag_id",
                table: "service_issue_has_tags");

            migrationBuilder.DropForeignKey(
                name: "fk_service_usergroups_groups_groups_group_id",
                table: "service_usergroups");

            migrationBuilder.DropForeignKey(
                name: "fk_service_usergroups_users_users_user_id",
                table: "service_usergroups");

            migrationBuilder.DropTable(
                name: "gameserver_playerpositions");

            migrationBuilder.DropTable(
                name: "gameserver_players");

            migrationBuilder.DropPrimaryKey(
                name: "pk_service_usergroups",
                table: "service_usergroups");

            migrationBuilder.DropPrimaryKey(
                name: "pk_service_grouppermissions",
                table: "service_grouppermissions");

            migrationBuilder.DropPrimaryKey(
                name: "pk_gameserver_whitelists",
                table: "gameserver_whitelists");

            migrationBuilder.DropPrimaryKey(
                name: "pk_gameserver_sessions",
                table: "gameserver_sessions");

            migrationBuilder.DropPrimaryKey(
                name: "pk_gameserver_playtimes",
                table: "gameserver_playtimes");

            migrationBuilder.DropIndex(
                name: "ix_gameserver_playtimes_player_id",
                table: "gameserver_playtimes");

            migrationBuilder.DropPrimaryKey(
                name: "pk_gameserver_playerwhitelistings",
                table: "gameserver_playerwhitelistings");

            migrationBuilder.DropIndex(
                name: "ix_gameserver_playerwhitelistings_players_player_id",
                table: "gameserver_playerwhitelistings");

            migrationBuilder.DropPrimaryKey(
                name: "pk_gameserver_missions",
                table: "gameserver_missions");

            migrationBuilder.DropPrimaryKey(
                name: "pk_gameserver_kills",
                table: "gameserver_kills");

            migrationBuilder.DropIndex(
                name: "ix_gameserver_kills_killer_player_id",
                table: "gameserver_kills");

            migrationBuilder.DropIndex(
                name: "ix_gameserver_kills_victim_player_id",
                table: "gameserver_kills");

            migrationBuilder.DropPrimaryKey(
                name: "pk_gameserver_chats",
                table: "gameserver_chats");

            migrationBuilder.DropPrimaryKey(
                name: "pk_gameserver_campaigns",
                table: "gameserver_campaigns");

            migrationBuilder.DropColumn(
                name: "play_time_id",
                table: "gameserver_playtimes");

            migrationBuilder.DropColumn(
                name: "players_player_id",
                table: "gameserver_playerwhitelistings");

            migrationBuilder.DropColumn(
                name: "killer_player_id",
                table: "gameserver_kills");

            migrationBuilder.DropColumn(
                name: "victim_player_id",
                table: "gameserver_kills");

            migrationBuilder.RenameTable(
                name: "service_users",
                newName: "ServiceUsers");

            migrationBuilder.RenameTable(
                name: "service_usergroups",
                newName: "ServiceUserGroups");

            migrationBuilder.RenameTable(
                name: "service_issues",
                newName: "ServiceIssues");

            migrationBuilder.RenameTable(
                name: "service_issue_tags",
                newName: "ServiceIssueTags");

            migrationBuilder.RenameTable(
                name: "service_issue_items",
                newName: "ServiceIssueItems");

            migrationBuilder.RenameTable(
                name: "service_issue_has_tags",
                newName: "ServiceIssueHasTags");

            migrationBuilder.RenameTable(
                name: "service_issue_groups",
                newName: "ServiceIssueGroups");

            migrationBuilder.RenameTable(
                name: "service_groups",
                newName: "ServiceGroups");

            migrationBuilder.RenameTable(
                name: "service_grouppermissions",
                newName: "ServiceGroupPermissions");

            migrationBuilder.RenameTable(
                name: "gameserver_whitelists",
                newName: "GameServerWhitelists");

            migrationBuilder.RenameTable(
                name: "gameserver_sessions",
                newName: "GameServerSessions");

            migrationBuilder.RenameTable(
                name: "gameserver_playtimes",
                newName: "GameServerPlaytimes");

            migrationBuilder.RenameTable(
                name: "gameserver_playerwhitelistings",
                newName: "GameServerPlayerWhitelistings");

            migrationBuilder.RenameTable(
                name: "gameserver_missions",
                newName: "GameServerMission");

            migrationBuilder.RenameTable(
                name: "gameserver_kills",
                newName: "GameServerKills");

            migrationBuilder.RenameTable(
                name: "gameserver_chats",
                newName: "GameServerChats");

            migrationBuilder.RenameTable(
                name: "gameserver_campaigns",
                newName: "GameServerCampaigns");

            migrationBuilder.RenameIndex(
                name: "ix_service_usergroups_users_user_id",
                table: "ServiceUserGroups",
                newName: "ix_service_user_groups_users_user_id");

            migrationBuilder.RenameIndex(
                name: "ix_service_grouppermissions_group_id",
                table: "ServiceGroupPermissions",
                newName: "ix_service_group_permissions_group_id");

            migrationBuilder.RenameIndex(
                name: "ix_gameserver_sessions_mission_id",
                table: "GameServerSessions",
                newName: "ix_game_server_sessions_mission_id");

            migrationBuilder.RenameIndex(
                name: "ix_gameserver_playtimes_session_id",
                table: "GameServerPlaytimes",
                newName: "ix_game_server_playtimes_session_id");

            migrationBuilder.RenameIndex(
                name: "ix_gameserver_missions_campaign_id",
                table: "GameServerMission",
                newName: "ix_game_server_mission_campaign_id");

            migrationBuilder.RenameIndex(
                name: "ix_gameserver_kills_session_id",
                table: "GameServerKills",
                newName: "ix_game_server_kills_session_id");

            migrationBuilder.RenameIndex(
                name: "ix_gameserver_chats_session_id",
                table: "GameServerChats",
                newName: "ix_game_server_chats_session_id");

            migrationBuilder.RenameIndex(
                name: "ix_gameserver_chats_player_id",
                table: "GameServerChats",
                newName: "ix_game_server_chats_player_id");

            migrationBuilder.RenameIndex(
                name: "ix_gameserver_chats_channel",
                table: "GameServerChats",
                newName: "ix_game_server_chats_channel");

            migrationBuilder.AlterColumn<long>(
                name: "mission_id",
                table: "GameServerSessions",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "session_id",
                table: "GameServerPlaytimes",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "player_id",
                table: "GameServerPlaytimes",
                type: "character varying(100)",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<string>(
                name: "players_player_uid",
                table: "GameServerPlayerWhitelistings",
                type: "character varying(100)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<long>(
                name: "campaign_id",
                table: "GameServerMission",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "session_id",
                table: "GameServerKills",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "killer_id",
                table: "GameServerKills",
                type: "character varying(100)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "victim_id",
                table: "GameServerKills",
                type: "character varying(100)",
                nullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "session_id",
                table: "GameServerChats",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "player_id",
                table: "GameServerChats",
                type: "character varying(100)",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "pk_service_user_groups",
                table: "ServiceUserGroups",
                columns: new[] { "groups_group_id", "users_user_id" });

            migrationBuilder.AddPrimaryKey(
                name: "pk_service_group_permissions",
                table: "ServiceGroupPermissions",
                column: "group_permission_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_game_server_whitelists",
                table: "GameServerWhitelists",
                column: "whitelist_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_game_server_sessions",
                table: "GameServerSessions",
                column: "session_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_game_server_playtimes",
                table: "GameServerPlaytimes",
                column: "player_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_game_server_player_whitelistings",
                table: "GameServerPlayerWhitelistings",
                columns: new[] { "player_whitelistings_whitelist_id", "players_player_uid" });

            migrationBuilder.AddPrimaryKey(
                name: "pk_game_server_mission",
                table: "GameServerMission",
                column: "mission_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_game_server_kills",
                table: "GameServerKills",
                column: "kill_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_game_server_chats",
                table: "GameServerChats",
                column: "chat_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_game_server_campaigns",
                table: "GameServerCampaigns",
                column: "campaign_id");

            migrationBuilder.CreateTable(
                name: "GameServerPlayers",
                columns: table => new
                {
                    player_uid = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    first_visit = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    last_visit = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_game_server_players", x => x.player_uid);
                });

            migrationBuilder.CreateTable(
                name: "GameServerReplayItems",
                columns: table => new
                {
                    replay_item_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    session_id = table.Column<long>(type: "bigint", nullable: false),
                    type = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    data = table.Column<string>(type: "jsonb", nullable: true),
                    game_tick_time = table.Column<long>(type: "bigint", nullable: false),
                    game_time = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_game_server_replay_items", x => x.replay_item_id);
                    table.ForeignKey(
                        name: "fk_game_server_replay_items_game_server_sessions_session_id",
                        column: x => x.session_id,
                        principalTable: "GameServerSessions",
                        principalColumn: "session_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ServiceApiKeys",
                columns: table => new
                {
                    api_key_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    api_key_value = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    owner_id = table.Column<Guid>(type: "uuid", nullable: false),
                    time_created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    valid_until = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_service_api_keys", x => x.api_key_id);
                    table.ForeignKey(
                        name: "fk_service_api_keys_service_users_owner_id",
                        column: x => x.owner_id,
                        principalTable: "ServiceUsers",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GameServerNotes",
                columns: table => new
                {
                    note_id = table.Column<long>(type: "bigint", nullable: false),
                    text = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    type = table.Column<string>(type: "text", nullable: true),
                    time_created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    time_last_update = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    player_id = table.Column<string>(type: "character varying(100)", nullable: true),
                    writer_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_game_server_notes", x => x.note_id);
                    table.ForeignKey(
                        name: "fk_game_server_notes_game_server_players_player_id",
                        column: x => x.player_id,
                        principalTable: "GameServerPlayers",
                        principalColumn: "player_uid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_game_server_notes_service_users_writer_id",
                        column: x => x.writer_id,
                        principalTable: "ServiceUsers",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_game_server_player_whitelistings_players_player_uid",
                table: "GameServerPlayerWhitelistings",
                column: "players_player_uid");

            migrationBuilder.CreateIndex(
                name: "ix_game_server_kills_killer_id",
                table: "GameServerKills",
                column: "killer_id");

            migrationBuilder.CreateIndex(
                name: "ix_game_server_kills_victim_id",
                table: "GameServerKills",
                column: "victim_id");

            migrationBuilder.CreateIndex(
                name: "ix_game_server_notes_player_id",
                table: "GameServerNotes",
                column: "player_id");

            migrationBuilder.CreateIndex(
                name: "ix_game_server_notes_writer_id",
                table: "GameServerNotes",
                column: "writer_id");

            migrationBuilder.CreateIndex(
                name: "ix_game_server_replay_items_session_id",
                table: "GameServerReplayItems",
                column: "session_id");

            migrationBuilder.CreateIndex(
                name: "ix_service_api_keys_owner_id",
                table: "ServiceApiKeys",
                column: "owner_id");

            migrationBuilder.AddForeignKey(
                name: "fk_game_server_chats_game_server_players_player_id",
                table: "GameServerChats",
                column: "player_id",
                principalTable: "GameServerPlayers",
                principalColumn: "player_uid",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_game_server_chats_game_server_sessions_session_id",
                table: "GameServerChats",
                column: "session_id",
                principalTable: "GameServerSessions",
                principalColumn: "session_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_game_server_kills_game_server_players_player_uid",
                table: "GameServerKills",
                column: "killer_id",
                principalTable: "GameServerPlayers",
                principalColumn: "player_uid",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_game_server_kills_game_server_players_victim_id",
                table: "GameServerKills",
                column: "victim_id",
                principalTable: "GameServerPlayers",
                principalColumn: "player_uid",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_game_server_kills_game_server_sessions_session_id",
                table: "GameServerKills",
                column: "session_id",
                principalTable: "GameServerSessions",
                principalColumn: "session_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_game_server_mission_game_server_campaigns_campaign_id",
                table: "GameServerMission",
                column: "campaign_id",
                principalTable: "GameServerCampaigns",
                principalColumn: "campaign_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_game_server_player_whitelistings_game_server_players_players_pla",
                table: "GameServerPlayerWhitelistings",
                column: "players_player_uid",
                principalTable: "GameServerPlayers",
                principalColumn: "player_uid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_game_server_player_whitelistings_game_server_whitelists_player_w",
                table: "GameServerPlayerWhitelistings",
                column: "player_whitelistings_whitelist_id",
                principalTable: "GameServerWhitelists",
                principalColumn: "whitelist_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_game_server_playtimes_game_server_players_player_id",
                table: "GameServerPlaytimes",
                column: "player_id",
                principalTable: "GameServerPlayers",
                principalColumn: "player_uid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_game_server_playtimes_game_server_sessions_session_id",
                table: "GameServerPlaytimes",
                column: "session_id",
                principalTable: "GameServerSessions",
                principalColumn: "session_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_game_server_sessions_game_server_mission_mission_id",
                table: "GameServerSessions",
                column: "mission_id",
                principalTable: "GameServerMission",
                principalColumn: "mission_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_service_group_permissions_service_groups_group_id",
                table: "ServiceGroupPermissions",
                column: "group_id",
                principalTable: "ServiceGroups",
                principalColumn: "group_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_service_issue_has_tags_service_issue_tags_issue_tags_issue_tag_id",
                table: "ServiceIssueHasTags",
                column: "issue_tags_issue_tag_id",
                principalTable: "ServiceIssueTags",
                principalColumn: "issue_tag_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_service_user_groups_service_groups_groups_group_id",
                table: "ServiceUserGroups",
                column: "groups_group_id",
                principalTable: "ServiceGroups",
                principalColumn: "group_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_service_user_groups_service_users_users_user_id",
                table: "ServiceUserGroups",
                column: "users_user_id",
                principalTable: "ServiceUsers",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_game_server_chats_game_server_players_player_id",
                table: "GameServerChats");

            migrationBuilder.DropForeignKey(
                name: "fk_game_server_chats_game_server_sessions_session_id",
                table: "GameServerChats");

            migrationBuilder.DropForeignKey(
                name: "fk_game_server_kills_game_server_players_player_uid",
                table: "GameServerKills");

            migrationBuilder.DropForeignKey(
                name: "fk_game_server_kills_game_server_players_victim_id",
                table: "GameServerKills");

            migrationBuilder.DropForeignKey(
                name: "fk_game_server_kills_game_server_sessions_session_id",
                table: "GameServerKills");

            migrationBuilder.DropForeignKey(
                name: "fk_game_server_mission_game_server_campaigns_campaign_id",
                table: "GameServerMission");

            migrationBuilder.DropForeignKey(
                name: "fk_game_server_player_whitelistings_game_server_players_players_pla",
                table: "GameServerPlayerWhitelistings");

            migrationBuilder.DropForeignKey(
                name: "fk_game_server_player_whitelistings_game_server_whitelists_player_w",
                table: "GameServerPlayerWhitelistings");

            migrationBuilder.DropForeignKey(
                name: "fk_game_server_playtimes_game_server_players_player_id",
                table: "GameServerPlaytimes");

            migrationBuilder.DropForeignKey(
                name: "fk_game_server_playtimes_game_server_sessions_session_id",
                table: "GameServerPlaytimes");

            migrationBuilder.DropForeignKey(
                name: "fk_game_server_sessions_game_server_mission_mission_id",
                table: "GameServerSessions");

            migrationBuilder.DropForeignKey(
                name: "fk_service_group_permissions_service_groups_group_id",
                table: "ServiceGroupPermissions");

            migrationBuilder.DropForeignKey(
                name: "fk_service_issue_has_tags_service_issue_tags_issue_tags_issue_tag_id",
                table: "ServiceIssueHasTags");

            migrationBuilder.DropForeignKey(
                name: "fk_service_user_groups_service_groups_groups_group_id",
                table: "ServiceUserGroups");

            migrationBuilder.DropForeignKey(
                name: "fk_service_user_groups_service_users_users_user_id",
                table: "ServiceUserGroups");

            migrationBuilder.DropTable(
                name: "GameServerNotes");

            migrationBuilder.DropTable(
                name: "GameServerReplayItems");

            migrationBuilder.DropTable(
                name: "ServiceApiKeys");

            migrationBuilder.DropTable(
                name: "GameServerPlayers");

            migrationBuilder.DropPrimaryKey(
                name: "pk_service_user_groups",
                table: "ServiceUserGroups");

            migrationBuilder.DropPrimaryKey(
                name: "pk_service_group_permissions",
                table: "ServiceGroupPermissions");

            migrationBuilder.DropPrimaryKey(
                name: "pk_game_server_whitelists",
                table: "GameServerWhitelists");

            migrationBuilder.DropPrimaryKey(
                name: "pk_game_server_sessions",
                table: "GameServerSessions");

            migrationBuilder.DropPrimaryKey(
                name: "pk_game_server_playtimes",
                table: "GameServerPlaytimes");

            migrationBuilder.DropPrimaryKey(
                name: "pk_game_server_player_whitelistings",
                table: "GameServerPlayerWhitelistings");

            migrationBuilder.DropIndex(
                name: "ix_game_server_player_whitelistings_players_player_uid",
                table: "GameServerPlayerWhitelistings");

            migrationBuilder.DropPrimaryKey(
                name: "pk_game_server_mission",
                table: "GameServerMission");

            migrationBuilder.DropPrimaryKey(
                name: "pk_game_server_kills",
                table: "GameServerKills");

            migrationBuilder.DropIndex(
                name: "ix_game_server_kills_killer_id",
                table: "GameServerKills");

            migrationBuilder.DropIndex(
                name: "ix_game_server_kills_victim_id",
                table: "GameServerKills");

            migrationBuilder.DropPrimaryKey(
                name: "pk_game_server_chats",
                table: "GameServerChats");

            migrationBuilder.DropPrimaryKey(
                name: "pk_game_server_campaigns",
                table: "GameServerCampaigns");

            migrationBuilder.DropColumn(
                name: "players_player_uid",
                table: "GameServerPlayerWhitelistings");

            migrationBuilder.DropColumn(
                name: "killer_id",
                table: "GameServerKills");

            migrationBuilder.DropColumn(
                name: "victim_id",
                table: "GameServerKills");

            migrationBuilder.RenameTable(
                name: "ServiceUsers",
                newName: "service_users");

            migrationBuilder.RenameTable(
                name: "ServiceUserGroups",
                newName: "service_usergroups");

            migrationBuilder.RenameTable(
                name: "ServiceIssueTags",
                newName: "service_issue_tags");

            migrationBuilder.RenameTable(
                name: "ServiceIssues",
                newName: "service_issues");

            migrationBuilder.RenameTable(
                name: "ServiceIssueItems",
                newName: "service_issue_items");

            migrationBuilder.RenameTable(
                name: "ServiceIssueHasTags",
                newName: "service_issue_has_tags");

            migrationBuilder.RenameTable(
                name: "ServiceIssueGroups",
                newName: "service_issue_groups");

            migrationBuilder.RenameTable(
                name: "ServiceGroups",
                newName: "service_groups");

            migrationBuilder.RenameTable(
                name: "ServiceGroupPermissions",
                newName: "service_grouppermissions");

            migrationBuilder.RenameTable(
                name: "GameServerWhitelists",
                newName: "gameserver_whitelists");

            migrationBuilder.RenameTable(
                name: "GameServerSessions",
                newName: "gameserver_sessions");

            migrationBuilder.RenameTable(
                name: "GameServerPlaytimes",
                newName: "gameserver_playtimes");

            migrationBuilder.RenameTable(
                name: "GameServerPlayerWhitelistings",
                newName: "gameserver_playerwhitelistings");

            migrationBuilder.RenameTable(
                name: "GameServerMission",
                newName: "gameserver_missions");

            migrationBuilder.RenameTable(
                name: "GameServerKills",
                newName: "gameserver_kills");

            migrationBuilder.RenameTable(
                name: "GameServerChats",
                newName: "gameserver_chats");

            migrationBuilder.RenameTable(
                name: "GameServerCampaigns",
                newName: "gameserver_campaigns");

            migrationBuilder.RenameIndex(
                name: "ix_service_user_groups_users_user_id",
                table: "service_usergroups",
                newName: "ix_service_usergroups_users_user_id");

            migrationBuilder.RenameIndex(
                name: "ix_service_group_permissions_group_id",
                table: "service_grouppermissions",
                newName: "ix_service_grouppermissions_group_id");

            migrationBuilder.RenameIndex(
                name: "ix_game_server_sessions_mission_id",
                table: "gameserver_sessions",
                newName: "ix_gameserver_sessions_mission_id");

            migrationBuilder.RenameIndex(
                name: "ix_game_server_playtimes_session_id",
                table: "gameserver_playtimes",
                newName: "ix_gameserver_playtimes_session_id");

            migrationBuilder.RenameIndex(
                name: "ix_game_server_mission_campaign_id",
                table: "gameserver_missions",
                newName: "ix_gameserver_missions_campaign_id");

            migrationBuilder.RenameIndex(
                name: "ix_game_server_kills_session_id",
                table: "gameserver_kills",
                newName: "ix_gameserver_kills_session_id");

            migrationBuilder.RenameIndex(
                name: "ix_game_server_chats_session_id",
                table: "gameserver_chats",
                newName: "ix_gameserver_chats_session_id");

            migrationBuilder.RenameIndex(
                name: "ix_game_server_chats_player_id",
                table: "gameserver_chats",
                newName: "ix_gameserver_chats_player_id");

            migrationBuilder.RenameIndex(
                name: "ix_game_server_chats_channel",
                table: "gameserver_chats",
                newName: "ix_gameserver_chats_channel");

            migrationBuilder.AlterColumn<long>(
                name: "mission_id",
                table: "gameserver_sessions",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "session_id",
                table: "gameserver_playtimes",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "player_id",
                table: "gameserver_playtimes",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)");

            migrationBuilder.AddColumn<long>(
                name: "play_time_id",
                table: "gameserver_playtimes",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<long>(
                name: "players_player_id",
                table: "gameserver_playerwhitelistings",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AlterColumn<long>(
                name: "campaign_id",
                table: "gameserver_missions",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "session_id",
                table: "gameserver_kills",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<long>(
                name: "killer_player_id",
                table: "gameserver_kills",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "victim_player_id",
                table: "gameserver_kills",
                type: "bigint",
                nullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "session_id",
                table: "gameserver_chats",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "player_id",
                table: "gameserver_chats",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "pk_service_usergroups",
                table: "service_usergroups",
                columns: new[] { "groups_group_id", "users_user_id" });

            migrationBuilder.AddPrimaryKey(
                name: "pk_service_grouppermissions",
                table: "service_grouppermissions",
                column: "group_permission_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_gameserver_whitelists",
                table: "gameserver_whitelists",
                column: "whitelist_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_gameserver_sessions",
                table: "gameserver_sessions",
                column: "session_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_gameserver_playtimes",
                table: "gameserver_playtimes",
                column: "play_time_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_gameserver_playerwhitelistings",
                table: "gameserver_playerwhitelistings",
                columns: new[] { "player_whitelistings_whitelist_id", "players_player_id" });

            migrationBuilder.AddPrimaryKey(
                name: "pk_gameserver_missions",
                table: "gameserver_missions",
                column: "mission_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_gameserver_kills",
                table: "gameserver_kills",
                column: "kill_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_gameserver_chats",
                table: "gameserver_chats",
                column: "chat_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_gameserver_campaigns",
                table: "gameserver_campaigns",
                column: "campaign_id");

            migrationBuilder.CreateTable(
                name: "gameserver_players",
                columns: table => new
                {
                    player_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    first_visit = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    last_visit = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    player_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    player_uid = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_gameserver_players", x => x.player_id);
                });

            migrationBuilder.CreateTable(
                name: "gameserver_playerpositions",
                columns: table => new
                {
                    position_tracker_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    direction = table.Column<long>(type: "bigint", nullable: false),
                    group = table.Column<string>(type: "text", nullable: true),
                    is_awake = table.Column<bool>(type: "boolean", nullable: false),
                    is_driver = table.Column<bool>(type: "boolean", nullable: false),
                    player_id = table.Column<long>(type: "bigint", nullable: true),
                    pos = table.Column<NpgsqlPoint>(type: "point", nullable: false),
                    session_id = table.Column<long>(type: "bigint", nullable: true),
                    side = table.Column<int>(type: "integer", nullable: false),
                    vehicle_name = table.Column<string>(type: "text", nullable: true),
                    vehicle_type = table.Column<int>(type: "integer", nullable: false),
                    velocity = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_gameserver_playerpositions", x => x.position_tracker_id);
                    table.ForeignKey(
                        name: "fk_gameserver_playerpositions_gameserver_players_player_id",
                        column: x => x.player_id,
                        principalTable: "gameserver_players",
                        principalColumn: "player_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_gameserver_playerpositions_gameserver_sessions_session_id",
                        column: x => x.session_id,
                        principalTable: "gameserver_sessions",
                        principalColumn: "session_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_gameserver_playtimes_player_id",
                table: "gameserver_playtimes",
                column: "player_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_gameserver_playerwhitelistings_players_player_id",
                table: "gameserver_playerwhitelistings",
                column: "players_player_id");

            migrationBuilder.CreateIndex(
                name: "ix_gameserver_kills_killer_player_id",
                table: "gameserver_kills",
                column: "killer_player_id");

            migrationBuilder.CreateIndex(
                name: "ix_gameserver_kills_victim_player_id",
                table: "gameserver_kills",
                column: "victim_player_id");

            migrationBuilder.CreateIndex(
                name: "ix_gameserver_playerpositions_player_id",
                table: "gameserver_playerpositions",
                column: "player_id");

            migrationBuilder.CreateIndex(
                name: "ix_gameserver_playerpositions_session_id",
                table: "gameserver_playerpositions",
                column: "session_id");

            migrationBuilder.AddForeignKey(
                name: "fk_gameserver_chats_gameserver_players_player_id",
                table: "gameserver_chats",
                column: "player_id",
                principalTable: "gameserver_players",
                principalColumn: "player_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_gameserver_chats_gameserver_sessions_session_id",
                table: "gameserver_chats",
                column: "session_id",
                principalTable: "gameserver_sessions",
                principalColumn: "session_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_gameserver_kills_gameserver_players_killer_player_id",
                table: "gameserver_kills",
                column: "killer_player_id",
                principalTable: "gameserver_players",
                principalColumn: "player_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_gameserver_kills_gameserver_players_victim_player_id",
                table: "gameserver_kills",
                column: "victim_player_id",
                principalTable: "gameserver_players",
                principalColumn: "player_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_gameserver_kills_gameserver_sessions_session_id",
                table: "gameserver_kills",
                column: "session_id",
                principalTable: "gameserver_sessions",
                principalColumn: "session_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_gameserver_missions_gameserver_campaigns_campaign_id",
                table: "gameserver_missions",
                column: "campaign_id",
                principalTable: "gameserver_campaigns",
                principalColumn: "campaign_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_gameserver_playerwhitelistings_players_players_player_id",
                table: "gameserver_playerwhitelistings",
                column: "players_player_id",
                principalTable: "gameserver_players",
                principalColumn: "player_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_gameserver_playerwhitelistings_whitelist_player_whitelistin",
                table: "gameserver_playerwhitelistings",
                column: "player_whitelistings_whitelist_id",
                principalTable: "gameserver_whitelists",
                principalColumn: "whitelist_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_gameserver_playtimes_gameserver_players_player_id",
                table: "gameserver_playtimes",
                column: "player_id",
                principalTable: "gameserver_players",
                principalColumn: "player_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_gameserver_playtimes_gameserver_sessions_session_id",
                table: "gameserver_playtimes",
                column: "session_id",
                principalTable: "gameserver_sessions",
                principalColumn: "session_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_gameserver_sessions_missions_mission_id",
                table: "gameserver_sessions",
                column: "mission_id",
                principalTable: "gameserver_missions",
                principalColumn: "mission_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_service_grouppermissions_service_groups_group_id",
                table: "service_grouppermissions",
                column: "group_id",
                principalTable: "service_groups",
                principalColumn: "group_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_service_issue_has_tags_issue_tags_issue_tags_issue_tag_id",
                table: "service_issue_has_tags",
                column: "issue_tags_issue_tag_id",
                principalTable: "service_issue_tags",
                principalColumn: "issue_tag_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_service_usergroups_groups_groups_group_id",
                table: "service_usergroups",
                column: "groups_group_id",
                principalTable: "service_groups",
                principalColumn: "group_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_service_usergroups_users_users_user_id",
                table: "service_usergroups",
                column: "users_user_id",
                principalTable: "service_users",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
