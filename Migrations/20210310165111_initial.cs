using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using NpgsqlTypes;

namespace TF47_Backend.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:side", "civilian,bluefor,redfor,independent")
                .Annotation("Npgsql:Enum:vehicle_type", "infantry,light_vehicle,tank,helicopter,fixed_wing,boat");

            migrationBuilder.CreateTable(
                name: "gameserver_campaigns",
                columns: table => new
                {
                    campaign_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    time_created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_gameserver_campaigns", x => x.campaign_id);
                });

            migrationBuilder.CreateTable(
                name: "gameserver_players",
                columns: table => new
                {
                    player_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    player_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    player_uid = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    first_visit = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    last_visit = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_gameserver_players", x => x.player_id);
                });

            migrationBuilder.CreateTable(
                name: "gameserver_whitelists",
                columns: table => new
                {
                    whitelist_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_gameserver_whitelists", x => x.whitelist_id);
                });

            migrationBuilder.CreateTable(
                name: "service_groups",
                columns: table => new
                {
                    group_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    text_color = table.Column<string>(type: "character varying(6)", maxLength: 6, nullable: false),
                    background_color = table.Column<string>(type: "character varying(6)", maxLength: 6, nullable: false),
                    is_visible = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_service_groups", x => x.group_id);
                });

            migrationBuilder.CreateTable(
                name: "service_users",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    banned = table.Column<bool>(type: "boolean", nullable: false),
                    mail = table.Column<string>(type: "text", nullable: true),
                    username = table.Column<string>(type: "text", nullable: false),
                    steam_id = table.Column<string>(type: "text", nullable: false),
                    country_code = table.Column<string>(type: "text", nullable: true),
                    profile_picture = table.Column<string>(type: "text", nullable: true),
                    profile_url = table.Column<string>(type: "text", nullable: true),
                    first_time_seen = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    last_time_seen = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_service_users", x => x.user_id);
                });

            migrationBuilder.CreateTable(
                name: "gameserver_missions",
                columns: table => new
                {
                    mission_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    mission_type = table.Column<int>(type: "integer", nullable: false),
                    campaign_id = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_gameserver_missions", x => x.mission_id);
                    table.ForeignKey(
                        name: "fk_gameserver_missions_gameserver_campaigns_campaign_id",
                        column: x => x.campaign_id,
                        principalTable: "gameserver_campaigns",
                        principalColumn: "campaign_id",
                        onDelete: ReferentialAction.Restrict);
                });

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
                name: "service_grouppermissions",
                columns: table => new
                {
                    group_permission_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    group_id = table.Column<long>(type: "bigint", nullable: false),
                    can_edit_groups = table.Column<bool>(type: "boolean", nullable: false),
                    can_ban_users = table.Column<bool>(type: "boolean", nullable: false),
                    can_ban_permanent = table.Column<bool>(type: "boolean", nullable: false),
                    can_edit_users = table.Column<bool>(type: "boolean", nullable: false),
                    can_delete_users = table.Column<bool>(type: "boolean", nullable: false),
                    can_edit_servers = table.Column<bool>(type: "boolean", nullable: false),
                    can_create_servers = table.Column<bool>(type: "boolean", nullable: false),
                    can_use_servers = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_service_grouppermissions", x => x.group_permission_id);
                    table.ForeignKey(
                        name: "fk_service_grouppermissions_service_groups_group_id",
                        column: x => x.group_id,
                        principalTable: "service_groups",
                        principalColumn: "group_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "service_userhasgroups",
                columns: table => new
                {
                    user_has_group_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<Guid>(type: "uuid", nullable: true),
                    group_id = table.Column<long>(type: "bigint", nullable: true),
                    time_added_to_group = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "gameserver_sessions",
                columns: table => new
                {
                    session_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    mission_id = table.Column<long>(type: "bigint", nullable: true),
                    world_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    mission_type = table.Column<int>(type: "integer", nullable: false),
                    session_created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    session_ended = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_gameserver_sessions", x => x.session_id);
                    table.ForeignKey(
                        name: "fk_gameserver_sessions_missions_mission_id",
                        column: x => x.mission_id,
                        principalTable: "gameserver_missions",
                        principalColumn: "mission_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "gameserver_chats",
                columns: table => new
                {
                    chat_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    channel = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    text = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    session_id = table.Column<long>(type: "bigint", nullable: true),
                    player_id = table.Column<long>(type: "bigint", nullable: true),
                    time_send = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_gameserver_chats", x => x.chat_id);
                    table.ForeignKey(
                        name: "fk_gameserver_chats_gameserver_players_player_id",
                        column: x => x.player_id,
                        principalTable: "gameserver_players",
                        principalColumn: "player_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_gameserver_chats_gameserver_sessions_session_id",
                        column: x => x.session_id,
                        principalTable: "gameserver_sessions",
                        principalColumn: "session_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "gameserver_kills",
                columns: table => new
                {
                    kill_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    session_id = table.Column<long>(type: "bigint", nullable: true),
                    killer_player_id = table.Column<long>(type: "bigint", nullable: true),
                    killer_vehicle_type = table.Column<int>(type: "integer", nullable: false),
                    killer_side = table.Column<int>(type: "integer", nullable: false),
                    victim_player_id = table.Column<long>(type: "bigint", nullable: true),
                    victim_vehicle_type = table.Column<int>(type: "integer", nullable: false),
                    victim_side = table.Column<int>(type: "integer", nullable: false),
                    weapon = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    vehicle_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    distance = table.Column<long>(type: "bigint", nullable: false),
                    game_time = table.Column<long>(type: "bigint", nullable: false),
                    real_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_gameserver_kills", x => x.kill_id);
                    table.ForeignKey(
                        name: "fk_gameserver_kills_gameserver_players_killer_player_id",
                        column: x => x.killer_player_id,
                        principalTable: "gameserver_players",
                        principalColumn: "player_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_gameserver_kills_gameserver_players_victim_player_id",
                        column: x => x.victim_player_id,
                        principalTable: "gameserver_players",
                        principalColumn: "player_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_gameserver_kills_gameserver_sessions_session_id",
                        column: x => x.session_id,
                        principalTable: "gameserver_sessions",
                        principalColumn: "session_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "gameserver_playerpositions",
                columns: table => new
                {
                    position_tracker_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    session_id = table.Column<long>(type: "bigint", nullable: true),
                    player_id = table.Column<long>(type: "bigint", nullable: true),
                    pos = table.Column<NpgsqlPoint>(type: "point", nullable: false),
                    side = table.Column<int>(type: "integer", nullable: false),
                    direction = table.Column<long>(type: "bigint", nullable: false),
                    velocity = table.Column<long>(type: "bigint", nullable: false),
                    group = table.Column<string>(type: "text", nullable: true),
                    vehicle_type = table.Column<int>(type: "integer", nullable: false),
                    vehicle_name = table.Column<string>(type: "text", nullable: true),
                    is_driver = table.Column<bool>(type: "boolean", nullable: false),
                    is_awake = table.Column<bool>(type: "boolean", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "gameserver_playtimes",
                columns: table => new
                {
                    play_time_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    player_id = table.Column<long>(type: "bigint", nullable: false),
                    session_id = table.Column<long>(type: "bigint", nullable: true),
                    time_played_infantry = table.Column<TimeSpan>(type: "interval", nullable: false),
                    time_played_vehicle = table.Column<TimeSpan>(type: "interval", nullable: false),
                    time_played_tank = table.Column<TimeSpan>(type: "interval", nullable: false),
                    time_played_helicopter = table.Column<TimeSpan>(type: "interval", nullable: false),
                    time_played_fixed_wing = table.Column<TimeSpan>(type: "interval", nullable: false),
                    time_played_boat = table.Column<TimeSpan>(type: "interval", nullable: false),
                    time_tracked_objective = table.Column<TimeSpan>(type: "interval", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_gameserver_playtimes", x => x.play_time_id);
                    table.ForeignKey(
                        name: "fk_gameserver_playtimes_gameserver_players_player_id",
                        column: x => x.player_id,
                        principalTable: "gameserver_players",
                        principalColumn: "player_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_gameserver_playtimes_gameserver_sessions_session_id",
                        column: x => x.session_id,
                        principalTable: "gameserver_sessions",
                        principalColumn: "session_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_gameserver_chats_channel",
                table: "gameserver_chats",
                column: "channel");

            migrationBuilder.CreateIndex(
                name: "ix_gameserver_chats_player_id",
                table: "gameserver_chats",
                column: "player_id");

            migrationBuilder.CreateIndex(
                name: "ix_gameserver_chats_session_id",
                table: "gameserver_chats",
                column: "session_id");

            migrationBuilder.CreateIndex(
                name: "ix_gameserver_kills_killer_player_id",
                table: "gameserver_kills",
                column: "killer_player_id");

            migrationBuilder.CreateIndex(
                name: "ix_gameserver_kills_session_id",
                table: "gameserver_kills",
                column: "session_id");

            migrationBuilder.CreateIndex(
                name: "ix_gameserver_kills_victim_player_id",
                table: "gameserver_kills",
                column: "victim_player_id");

            migrationBuilder.CreateIndex(
                name: "ix_gameserver_missions_campaign_id",
                table: "gameserver_missions",
                column: "campaign_id");

            migrationBuilder.CreateIndex(
                name: "ix_gameserver_playerpositions_player_id",
                table: "gameserver_playerpositions",
                column: "player_id");

            migrationBuilder.CreateIndex(
                name: "ix_gameserver_playerpositions_session_id",
                table: "gameserver_playerpositions",
                column: "session_id");

            migrationBuilder.CreateIndex(
                name: "ix_gameserver_playtimes_player_id",
                table: "gameserver_playtimes",
                column: "player_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_gameserver_playtimes_session_id",
                table: "gameserver_playtimes",
                column: "session_id");

            migrationBuilder.CreateIndex(
                name: "ix_gameserver_sessions_mission_id",
                table: "gameserver_sessions",
                column: "mission_id");

            migrationBuilder.CreateIndex(
                name: "ix_gameserver_whitelistings_player_id",
                table: "gameserver_whitelistings",
                column: "player_id");

            migrationBuilder.CreateIndex(
                name: "ix_gameserver_whitelistings_whitelist_id",
                table: "gameserver_whitelistings",
                column: "whitelist_id");

            migrationBuilder.CreateIndex(
                name: "ix_service_grouppermissions_group_id",
                table: "service_grouppermissions",
                column: "group_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_service_userhasgroups_group_id",
                table: "service_userhasgroups",
                column: "group_id");

            migrationBuilder.CreateIndex(
                name: "ix_service_userhasgroups_user_id",
                table: "service_userhasgroups",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_service_users_steam_id",
                table: "service_users",
                column: "steam_id",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "gameserver_chats");

            migrationBuilder.DropTable(
                name: "gameserver_kills");

            migrationBuilder.DropTable(
                name: "gameserver_playerpositions");

            migrationBuilder.DropTable(
                name: "gameserver_playtimes");

            migrationBuilder.DropTable(
                name: "gameserver_whitelistings");

            migrationBuilder.DropTable(
                name: "service_grouppermissions");

            migrationBuilder.DropTable(
                name: "service_userhasgroups");

            migrationBuilder.DropTable(
                name: "gameserver_sessions");

            migrationBuilder.DropTable(
                name: "gameserver_players");

            migrationBuilder.DropTable(
                name: "gameserver_whitelists");

            migrationBuilder.DropTable(
                name: "service_groups");

            migrationBuilder.DropTable(
                name: "service_users");

            migrationBuilder.DropTable(
                name: "gameserver_missions");

            migrationBuilder.DropTable(
                name: "gameserver_campaigns");
        }
    }
}
