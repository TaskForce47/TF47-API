using Microsoft.EntityFrameworkCore.Migrations;

namespace TF47_Backend.Migrations
{
    public partial class table_prefix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_chats_players_player_id",
                table: "chats");

            migrationBuilder.DropForeignKey(
                name: "fk_chats_sessions_session_id",
                table: "chats");

            migrationBuilder.DropForeignKey(
                name: "fk_group_permissions_groups_group_id",
                table: "group_permissions");

            migrationBuilder.DropForeignKey(
                name: "fk_groups_users_user_id",
                table: "groups");

            migrationBuilder.DropForeignKey(
                name: "fk_kills_players_player_id",
                table: "kills");

            migrationBuilder.DropForeignKey(
                name: "fk_kills_players_victim_player_id",
                table: "kills");

            migrationBuilder.DropForeignKey(
                name: "fk_kills_sessions_session_id",
                table: "kills");

            migrationBuilder.DropForeignKey(
                name: "fk_missions_campaigns_campaign_id",
                table: "missions");

            migrationBuilder.DropForeignKey(
                name: "fk_playtimes_players_player_id",
                table: "playtimes");

            migrationBuilder.DropForeignKey(
                name: "fk_playtimes_sessions_session_id",
                table: "playtimes");

            migrationBuilder.DropForeignKey(
                name: "fk_positions_players_player_id",
                table: "positions");

            migrationBuilder.DropForeignKey(
                name: "fk_positions_sessions_session_id",
                table: "positions");

            migrationBuilder.DropForeignKey(
                name: "fk_sessions_missions_mission_id",
                table: "sessions");

            migrationBuilder.DropForeignKey(
                name: "fk_whitelisting_players_player_id",
                table: "whitelisting");

            migrationBuilder.DropForeignKey(
                name: "fk_whitelisting_whitelist_whitelist_id",
                table: "whitelisting");

            migrationBuilder.DropPrimaryKey(
                name: "pk_whitelisting",
                table: "whitelisting");

            migrationBuilder.DropPrimaryKey(
                name: "pk_whitelist",
                table: "whitelist");

            migrationBuilder.DropPrimaryKey(
                name: "pk_users",
                table: "users");

            migrationBuilder.DropPrimaryKey(
                name: "pk_sessions",
                table: "sessions");

            migrationBuilder.DropPrimaryKey(
                name: "pk_positions",
                table: "positions");

            migrationBuilder.DropPrimaryKey(
                name: "pk_playtimes",
                table: "playtimes");

            migrationBuilder.DropPrimaryKey(
                name: "pk_players",
                table: "players");

            migrationBuilder.DropPrimaryKey(
                name: "pk_missions",
                table: "missions");

            migrationBuilder.DropPrimaryKey(
                name: "pk_kills",
                table: "kills");

            migrationBuilder.DropPrimaryKey(
                name: "pk_groups",
                table: "groups");

            migrationBuilder.DropPrimaryKey(
                name: "pk_group_permissions",
                table: "group_permissions");

            migrationBuilder.DropPrimaryKey(
                name: "pk_chats",
                table: "chats");

            migrationBuilder.DropPrimaryKey(
                name: "pk_campaigns",
                table: "campaigns");

            migrationBuilder.RenameTable(
                name: "whitelisting",
                newName: "gameserver_whitelistings");

            migrationBuilder.RenameTable(
                name: "whitelist",
                newName: "gameserver_whitelists");

            migrationBuilder.RenameTable(
                name: "users",
                newName: "service_players");

            migrationBuilder.RenameTable(
                name: "sessions",
                newName: "gameserver_sessions");

            migrationBuilder.RenameTable(
                name: "positions",
                newName: "gameserver_playerpositions");

            migrationBuilder.RenameTable(
                name: "playtimes",
                newName: "gameserver_playtimes");

            migrationBuilder.RenameTable(
                name: "players",
                newName: "gameserver_players");

            migrationBuilder.RenameTable(
                name: "missions",
                newName: "gameserver_missions");

            migrationBuilder.RenameTable(
                name: "kills",
                newName: "gameserver_kills");

            migrationBuilder.RenameTable(
                name: "groups",
                newName: "service_groups");

            migrationBuilder.RenameTable(
                name: "group_permissions",
                newName: "service_grouppermissions");

            migrationBuilder.RenameTable(
                name: "chats",
                newName: "gameserver_chats");

            migrationBuilder.RenameTable(
                name: "campaigns",
                newName: "gameserver_campaigns");

            migrationBuilder.RenameIndex(
                name: "ix_whitelisting_whitelist_id",
                table: "gameserver_whitelistings",
                newName: "ix_gameserver_whitelistings_whitelist_id");

            migrationBuilder.RenameIndex(
                name: "ix_whitelisting_player_id",
                table: "gameserver_whitelistings",
                newName: "ix_gameserver_whitelistings_player_id");

            migrationBuilder.RenameIndex(
                name: "ix_sessions_mission_id",
                table: "gameserver_sessions",
                newName: "ix_gameserver_sessions_mission_id");

            migrationBuilder.RenameIndex(
                name: "ix_positions_session_id",
                table: "gameserver_playerpositions",
                newName: "ix_gameserver_playerpositions_session_id");

            migrationBuilder.RenameIndex(
                name: "ix_positions_player_id",
                table: "gameserver_playerpositions",
                newName: "ix_gameserver_playerpositions_player_id");

            migrationBuilder.RenameIndex(
                name: "ix_playtimes_session_id",
                table: "gameserver_playtimes",
                newName: "ix_gameserver_playtimes_session_id");

            migrationBuilder.RenameIndex(
                name: "ix_playtimes_player_id",
                table: "gameserver_playtimes",
                newName: "ix_gameserver_playtimes_player_id");

            migrationBuilder.RenameIndex(
                name: "ix_missions_campaign_id",
                table: "gameserver_missions",
                newName: "ix_gameserver_missions_campaign_id");

            migrationBuilder.RenameIndex(
                name: "ix_kills_victim_player_id",
                table: "gameserver_kills",
                newName: "ix_gameserver_kills_victim_player_id");

            migrationBuilder.RenameIndex(
                name: "ix_kills_session_id",
                table: "gameserver_kills",
                newName: "ix_gameserver_kills_session_id");

            migrationBuilder.RenameIndex(
                name: "ix_kills_killer_player_id",
                table: "gameserver_kills",
                newName: "ix_gameserver_kills_killer_player_id");

            migrationBuilder.RenameIndex(
                name: "ix_groups_user_id",
                table: "service_groups",
                newName: "ix_service_groups_user_id");

            migrationBuilder.RenameIndex(
                name: "ix_group_permissions_group_id",
                table: "service_grouppermissions",
                newName: "ix_service_grouppermissions_group_id");

            migrationBuilder.RenameIndex(
                name: "ix_chats_session_id",
                table: "gameserver_chats",
                newName: "ix_gameserver_chats_session_id");

            migrationBuilder.RenameIndex(
                name: "ix_chats_player_id",
                table: "gameserver_chats",
                newName: "ix_gameserver_chats_player_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_gameserver_whitelistings",
                table: "gameserver_whitelistings",
                column: "whitelisting_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_gameserver_whitelists",
                table: "gameserver_whitelists",
                column: "whitelist_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_service_players",
                table: "service_players",
                column: "user_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_gameserver_sessions",
                table: "gameserver_sessions",
                column: "session_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_gameserver_playerpositions",
                table: "gameserver_playerpositions",
                column: "position_tracker_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_gameserver_playtimes",
                table: "gameserver_playtimes",
                column: "play_time_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_gameserver_players",
                table: "gameserver_players",
                column: "player_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_gameserver_missions",
                table: "gameserver_missions",
                column: "mission_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_gameserver_kills",
                table: "gameserver_kills",
                column: "kill_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_service_groups",
                table: "service_groups",
                column: "group_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_service_grouppermissions",
                table: "service_grouppermissions",
                column: "group_permission_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_gameserver_chats",
                table: "gameserver_chats",
                column: "chat_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_gameserver_campaigns",
                table: "gameserver_campaigns",
                column: "campaign_id");

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
                name: "fk_gameserver_playerpositions_gameserver_players_player_id",
                table: "gameserver_playerpositions",
                column: "player_id",
                principalTable: "gameserver_players",
                principalColumn: "player_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_gameserver_playerpositions_gameserver_sessions_session_id",
                table: "gameserver_playerpositions",
                column: "session_id",
                principalTable: "gameserver_sessions",
                principalColumn: "session_id",
                onDelete: ReferentialAction.Restrict);

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
                name: "fk_gameserver_whitelistings_gameserver_players_player_id",
                table: "gameserver_whitelistings",
                column: "player_id",
                principalTable: "gameserver_players",
                principalColumn: "player_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_gameserver_whitelistings_gameserver_whitelists_whitelist_id",
                table: "gameserver_whitelistings",
                column: "whitelist_id",
                principalTable: "gameserver_whitelists",
                principalColumn: "whitelist_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_service_grouppermissions_service_groups_group_id",
                table: "service_grouppermissions",
                column: "group_id",
                principalTable: "service_groups",
                principalColumn: "group_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_service_groups_service_players_user_id",
                table: "service_groups",
                column: "user_id",
                principalTable: "service_players",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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
                name: "fk_gameserver_playerpositions_gameserver_players_player_id",
                table: "gameserver_playerpositions");

            migrationBuilder.DropForeignKey(
                name: "fk_gameserver_playerpositions_gameserver_sessions_session_id",
                table: "gameserver_playerpositions");

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
                name: "fk_gameserver_whitelistings_gameserver_players_player_id",
                table: "gameserver_whitelistings");

            migrationBuilder.DropForeignKey(
                name: "fk_gameserver_whitelistings_gameserver_whitelists_whitelist_id",
                table: "gameserver_whitelistings");

            migrationBuilder.DropForeignKey(
                name: "fk_service_grouppermissions_service_groups_group_id",
                table: "service_grouppermissions");

            migrationBuilder.DropForeignKey(
                name: "fk_service_groups_service_players_user_id",
                table: "service_groups");

            migrationBuilder.DropPrimaryKey(
                name: "pk_service_players",
                table: "service_players");

            migrationBuilder.DropPrimaryKey(
                name: "pk_service_groups",
                table: "service_groups");

            migrationBuilder.DropPrimaryKey(
                name: "pk_service_grouppermissions",
                table: "service_grouppermissions");

            migrationBuilder.DropPrimaryKey(
                name: "pk_gameserver_whitelists",
                table: "gameserver_whitelists");

            migrationBuilder.DropPrimaryKey(
                name: "pk_gameserver_whitelistings",
                table: "gameserver_whitelistings");

            migrationBuilder.DropPrimaryKey(
                name: "pk_gameserver_sessions",
                table: "gameserver_sessions");

            migrationBuilder.DropPrimaryKey(
                name: "pk_gameserver_playtimes",
                table: "gameserver_playtimes");

            migrationBuilder.DropPrimaryKey(
                name: "pk_gameserver_players",
                table: "gameserver_players");

            migrationBuilder.DropPrimaryKey(
                name: "pk_gameserver_playerpositions",
                table: "gameserver_playerpositions");

            migrationBuilder.DropPrimaryKey(
                name: "pk_gameserver_missions",
                table: "gameserver_missions");

            migrationBuilder.DropPrimaryKey(
                name: "pk_gameserver_kills",
                table: "gameserver_kills");

            migrationBuilder.DropPrimaryKey(
                name: "pk_gameserver_chats",
                table: "gameserver_chats");

            migrationBuilder.DropPrimaryKey(
                name: "pk_gameserver_campaigns",
                table: "gameserver_campaigns");

            migrationBuilder.RenameTable(
                name: "service_players",
                newName: "users");

            migrationBuilder.RenameTable(
                name: "service_groups",
                newName: "groups");

            migrationBuilder.RenameTable(
                name: "service_grouppermissions",
                newName: "group_permissions");

            migrationBuilder.RenameTable(
                name: "gameserver_whitelists",
                newName: "whitelist");

            migrationBuilder.RenameTable(
                name: "gameserver_whitelistings",
                newName: "whitelisting");

            migrationBuilder.RenameTable(
                name: "gameserver_sessions",
                newName: "sessions");

            migrationBuilder.RenameTable(
                name: "gameserver_playtimes",
                newName: "playtimes");

            migrationBuilder.RenameTable(
                name: "gameserver_players",
                newName: "players");

            migrationBuilder.RenameTable(
                name: "gameserver_playerpositions",
                newName: "positions");

            migrationBuilder.RenameTable(
                name: "gameserver_missions",
                newName: "missions");

            migrationBuilder.RenameTable(
                name: "gameserver_kills",
                newName: "kills");

            migrationBuilder.RenameTable(
                name: "gameserver_chats",
                newName: "chats");

            migrationBuilder.RenameTable(
                name: "gameserver_campaigns",
                newName: "campaigns");

            migrationBuilder.RenameIndex(
                name: "ix_service_groups_user_id",
                table: "groups",
                newName: "ix_groups_user_id");

            migrationBuilder.RenameIndex(
                name: "ix_service_grouppermissions_group_id",
                table: "group_permissions",
                newName: "ix_group_permissions_group_id");

            migrationBuilder.RenameIndex(
                name: "ix_gameserver_whitelistings_whitelist_id",
                table: "whitelisting",
                newName: "ix_whitelisting_whitelist_id");

            migrationBuilder.RenameIndex(
                name: "ix_gameserver_whitelistings_player_id",
                table: "whitelisting",
                newName: "ix_whitelisting_player_id");

            migrationBuilder.RenameIndex(
                name: "ix_gameserver_sessions_mission_id",
                table: "sessions",
                newName: "ix_sessions_mission_id");

            migrationBuilder.RenameIndex(
                name: "ix_gameserver_playtimes_session_id",
                table: "playtimes",
                newName: "ix_playtimes_session_id");

            migrationBuilder.RenameIndex(
                name: "ix_gameserver_playtimes_player_id",
                table: "playtimes",
                newName: "ix_playtimes_player_id");

            migrationBuilder.RenameIndex(
                name: "ix_gameserver_playerpositions_session_id",
                table: "positions",
                newName: "ix_positions_session_id");

            migrationBuilder.RenameIndex(
                name: "ix_gameserver_playerpositions_player_id",
                table: "positions",
                newName: "ix_positions_player_id");

            migrationBuilder.RenameIndex(
                name: "ix_gameserver_missions_campaign_id",
                table: "missions",
                newName: "ix_missions_campaign_id");

            migrationBuilder.RenameIndex(
                name: "ix_gameserver_kills_victim_player_id",
                table: "kills",
                newName: "ix_kills_victim_player_id");

            migrationBuilder.RenameIndex(
                name: "ix_gameserver_kills_session_id",
                table: "kills",
                newName: "ix_kills_session_id");

            migrationBuilder.RenameIndex(
                name: "ix_gameserver_kills_killer_player_id",
                table: "kills",
                newName: "ix_kills_killer_player_id");

            migrationBuilder.RenameIndex(
                name: "ix_gameserver_chats_session_id",
                table: "chats",
                newName: "ix_chats_session_id");

            migrationBuilder.RenameIndex(
                name: "ix_gameserver_chats_player_id",
                table: "chats",
                newName: "ix_chats_player_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_users",
                table: "users",
                column: "user_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_groups",
                table: "groups",
                column: "group_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_group_permissions",
                table: "group_permissions",
                column: "group_permission_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_whitelist",
                table: "whitelist",
                column: "whitelist_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_whitelisting",
                table: "whitelisting",
                column: "whitelisting_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_sessions",
                table: "sessions",
                column: "session_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_playtimes",
                table: "playtimes",
                column: "play_time_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_players",
                table: "players",
                column: "player_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_positions",
                table: "positions",
                column: "position_tracker_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_missions",
                table: "missions",
                column: "mission_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_kills",
                table: "kills",
                column: "kill_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_chats",
                table: "chats",
                column: "chat_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_campaigns",
                table: "campaigns",
                column: "campaign_id");

            migrationBuilder.AddForeignKey(
                name: "fk_chats_players_player_id",
                table: "chats",
                column: "player_id",
                principalTable: "players",
                principalColumn: "player_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_chats_sessions_session_id",
                table: "chats",
                column: "session_id",
                principalTable: "sessions",
                principalColumn: "session_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_group_permissions_groups_group_id",
                table: "group_permissions",
                column: "group_id",
                principalTable: "groups",
                principalColumn: "group_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_groups_users_user_id",
                table: "groups",
                column: "user_id",
                principalTable: "users",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_kills_players_player_id",
                table: "kills",
                column: "killer_player_id",
                principalTable: "players",
                principalColumn: "player_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_kills_players_victim_player_id",
                table: "kills",
                column: "victim_player_id",
                principalTable: "players",
                principalColumn: "player_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_kills_sessions_session_id",
                table: "kills",
                column: "session_id",
                principalTable: "sessions",
                principalColumn: "session_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_missions_campaigns_campaign_id",
                table: "missions",
                column: "campaign_id",
                principalTable: "campaigns",
                principalColumn: "campaign_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_playtimes_players_player_id",
                table: "playtimes",
                column: "player_id",
                principalTable: "players",
                principalColumn: "player_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_playtimes_sessions_session_id",
                table: "playtimes",
                column: "session_id",
                principalTable: "sessions",
                principalColumn: "session_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_positions_players_player_id",
                table: "positions",
                column: "player_id",
                principalTable: "players",
                principalColumn: "player_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_positions_sessions_session_id",
                table: "positions",
                column: "session_id",
                principalTable: "sessions",
                principalColumn: "session_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_sessions_missions_mission_id",
                table: "sessions",
                column: "mission_id",
                principalTable: "missions",
                principalColumn: "mission_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_whitelisting_players_player_id",
                table: "whitelisting",
                column: "player_id",
                principalTable: "players",
                principalColumn: "player_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_whitelisting_whitelist_whitelist_id",
                table: "whitelisting",
                column: "whitelist_id",
                principalTable: "whitelist",
                principalColumn: "whitelist_id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
