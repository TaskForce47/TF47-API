using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NpgsqlTypes;

namespace TF47_Backend.Migrations
{
    public partial class inital_migration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:side", "civilian,bluefor,redfor,independent")
                .Annotation("Npgsql:Enum:vehicle_type", "infantry,light_vehicle,tank,helicopter,fixed_wing,boat");

            migrationBuilder.CreateTable(
                name: "campaigns",
                columns: table => new
                {
                    campaign_id = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    time_created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_campaigns", x => x.campaign_id);
                });

            migrationBuilder.CreateTable(
                name: "players",
                columns: table => new
                {
                    player_id = table.Column<long>(type: "bigint", nullable: false),
                    player_name = table.Column<string>(type: "text", nullable: true),
                    player_uid = table.Column<string>(type: "text", nullable: true),
                    first_visit = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    last_visit = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_players", x => x.player_id);
                });

            migrationBuilder.CreateTable(
                name: "whitelist",
                columns: table => new
                {
                    whitelist_id = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_whitelist", x => x.whitelist_id);
                });

            migrationBuilder.CreateTable(
                name: "missions",
                columns: table => new
                {
                    mission_id = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    mission_type = table.Column<int>(type: "integer", nullable: false),
                    campaign_id = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_missions", x => x.mission_id);
                    table.ForeignKey(
                        name: "fk_missions_campaigns_campaign_id",
                        column: x => x.campaign_id,
                        principalTable: "campaigns",
                        principalColumn: "campaign_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "whitelisting",
                columns: table => new
                {
                    whitelisting_id = table.Column<long>(type: "bigint", nullable: false),
                    player_id = table.Column<long>(type: "bigint", nullable: true),
                    whitelist_id = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_whitelisting", x => x.whitelisting_id);
                    table.ForeignKey(
                        name: "fk_whitelisting_players_player_id",
                        column: x => x.player_id,
                        principalTable: "players",
                        principalColumn: "player_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_whitelisting_whitelist_whitelist_id",
                        column: x => x.whitelist_id,
                        principalTable: "whitelist",
                        principalColumn: "whitelist_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "sessions",
                columns: table => new
                {
                    session_id = table.Column<long>(type: "bigint", nullable: false),
                    mission_id = table.Column<long>(type: "bigint", nullable: true),
                    world_name = table.Column<string>(type: "text", nullable: true),
                    mission_type = table.Column<int>(type: "integer", nullable: false),
                    session_created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    session_ended = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_sessions", x => x.session_id);
                    table.ForeignKey(
                        name: "fk_sessions_missions_mission_id",
                        column: x => x.mission_id,
                        principalTable: "missions",
                        principalColumn: "mission_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "kills",
                columns: table => new
                {
                    kill_id = table.Column<long>(type: "bigint", nullable: false),
                    session_id = table.Column<long>(type: "bigint", nullable: true),
                    killer_player_id = table.Column<long>(type: "bigint", nullable: true),
                    killer_vehicle_type = table.Column<int>(type: "integer", nullable: false),
                    killer_side = table.Column<int>(type: "integer", nullable: false),
                    victim_player_id = table.Column<long>(type: "bigint", nullable: true),
                    victim_vehicle_type = table.Column<int>(type: "integer", nullable: false),
                    victim_side = table.Column<int>(type: "integer", nullable: false),
                    weapon = table.Column<string>(type: "text", nullable: true),
                    vehicle_name = table.Column<string>(type: "text", nullable: true),
                    distance = table.Column<long>(type: "bigint", nullable: false),
                    game_time = table.Column<long>(type: "bigint", nullable: false),
                    real_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_kills", x => x.kill_id);
                    table.ForeignKey(
                        name: "fk_kills_players_player_id",
                        column: x => x.killer_player_id,
                        principalTable: "players",
                        principalColumn: "player_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_kills_players_victim_player_id",
                        column: x => x.victim_player_id,
                        principalTable: "players",
                        principalColumn: "player_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_kills_sessions_session_id",
                        column: x => x.session_id,
                        principalTable: "sessions",
                        principalColumn: "session_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "playtimes",
                columns: table => new
                {
                    play_time_id = table.Column<long>(type: "bigint", nullable: false),
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
                    table.PrimaryKey("pk_playtimes", x => x.play_time_id);
                    table.ForeignKey(
                        name: "fk_playtimes_players_player_id",
                        column: x => x.player_id,
                        principalTable: "players",
                        principalColumn: "player_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_playtimes_sessions_session_id",
                        column: x => x.session_id,
                        principalTable: "sessions",
                        principalColumn: "session_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "positions",
                columns: table => new
                {
                    position_tracker_id = table.Column<long>(type: "bigint", nullable: false),
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
                    table.PrimaryKey("pk_positions", x => x.position_tracker_id);
                    table.ForeignKey(
                        name: "fk_positions_players_player_id",
                        column: x => x.player_id,
                        principalTable: "players",
                        principalColumn: "player_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_positions_sessions_session_id",
                        column: x => x.session_id,
                        principalTable: "sessions",
                        principalColumn: "session_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_kills_killer_player_id",
                table: "kills",
                column: "killer_player_id");

            migrationBuilder.CreateIndex(
                name: "ix_kills_session_id",
                table: "kills",
                column: "session_id");

            migrationBuilder.CreateIndex(
                name: "ix_kills_victim_player_id",
                table: "kills",
                column: "victim_player_id");

            migrationBuilder.CreateIndex(
                name: "ix_missions_campaign_id",
                table: "missions",
                column: "campaign_id");

            migrationBuilder.CreateIndex(
                name: "ix_playtimes_player_id",
                table: "playtimes",
                column: "player_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_playtimes_session_id",
                table: "playtimes",
                column: "session_id");

            migrationBuilder.CreateIndex(
                name: "ix_positions_player_id",
                table: "positions",
                column: "player_id");

            migrationBuilder.CreateIndex(
                name: "ix_positions_session_id",
                table: "positions",
                column: "session_id");

            migrationBuilder.CreateIndex(
                name: "ix_sessions_mission_id",
                table: "sessions",
                column: "mission_id");

            migrationBuilder.CreateIndex(
                name: "ix_whitelisting_player_id",
                table: "whitelisting",
                column: "player_id");

            migrationBuilder.CreateIndex(
                name: "ix_whitelisting_whitelist_id",
                table: "whitelisting",
                column: "whitelist_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "kills");

            migrationBuilder.DropTable(
                name: "playtimes");

            migrationBuilder.DropTable(
                name: "positions");

            migrationBuilder.DropTable(
                name: "whitelisting");

            migrationBuilder.DropTable(
                name: "sessions");

            migrationBuilder.DropTable(
                name: "players");

            migrationBuilder.DropTable(
                name: "whitelist");

            migrationBuilder.DropTable(
                name: "missions");

            migrationBuilder.DropTable(
                name: "campaigns");
        }
    }
}
