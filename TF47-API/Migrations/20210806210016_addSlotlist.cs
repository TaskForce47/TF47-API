using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace TF47_API.Migrations
{
    public partial class addSlotlist : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GameServerConfig",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    host_name = table.Column<string>(type: "text", nullable: true),
                    server_password = table.Column<string>(type: "text", nullable: true),
                    admin_password = table.Column<string>(type: "text", nullable: true),
                    command_password = table.Column<string>(type: "text", nullable: true),
                    motd_messages = table.Column<string[]>(type: "text[]", nullable: true),
                    motd_interval = table.Column<long>(type: "bigint", nullable: false),
                    admin_steam_ui_ds = table.Column<string[]>(type: "text[]", nullable: true),
                    steam_max_protocol_size = table.Column<long>(type: "bigint", nullable: false),
                    max_players = table.Column<long>(type: "bigint", nullable: false),
                    kick_duplicate = table.Column<bool>(type: "boolean", nullable: false),
                    verify_signatures = table.Column<bool>(type: "boolean", nullable: false),
                    disconnect_timeout = table.Column<long>(type: "bigint", nullable: false),
                    max_deync = table.Column<long>(type: "bigint", nullable: false),
                    max_ping = table.Column<long>(type: "bigint", nullable: false),
                    max_packet_loss = table.Column<long>(type: "bigint", nullable: false),
                    enable_player_diag = table.Column<bool>(type: "boolean", nullable: false),
                    allow_file_patching = table.Column<bool>(type: "boolean", nullable: false),
                    enable_battleye = table.Column<bool>(type: "boolean", nullable: false),
                    allow_map_drawing = table.Column<bool>(type: "boolean", nullable: false),
                    persistence = table.Column<bool>(type: "boolean", nullable: false),
                    advanced_flight_model = table.Column<int>(type: "integer", nullable: false),
                    vote_threshold = table.Column<long>(type: "bigint", nullable: false),
                    vote_mission_players = table.Column<long>(type: "bigint", nullable: false),
                    disable_von = table.Column<bool>(type: "boolean", nullable: false),
                    audio_codec_setting = table.Column<int>(type: "integer", nullable: false),
                    von_quality = table.Column<long>(type: "bigint", nullable: false),
                    force_difficulty = table.Column<bool>(type: "boolean", nullable: false),
                    difficulty_setting = table.Column<int>(type: "integer", nullable: false),
                    reduced_damage = table.Column<bool>(type: "boolean", nullable: false),
                    group_indicators = table.Column<int>(type: "integer", nullable: false),
                    friendly_tags = table.Column<int>(type: "integer", nullable: false),
                    enemy_tags = table.Column<int>(type: "integer", nullable: false),
                    detect_mines = table.Column<int>(type: "integer", nullable: false),
                    commands = table.Column<int>(type: "integer", nullable: false),
                    waypoints = table.Column<int>(type: "integer", nullable: false),
                    weapon_info = table.Column<int>(type: "integer", nullable: false),
                    stance_indicator = table.Column<int>(type: "integer", nullable: false),
                    tactical_ping = table.Column<bool>(type: "boolean", nullable: false),
                    stamina_bar = table.Column<bool>(type: "boolean", nullable: false),
                    weapon_crosshair = table.Column<bool>(type: "boolean", nullable: false),
                    vision_aid = table.Column<bool>(type: "boolean", nullable: false),
                    third_person_view = table.Column<bool>(type: "boolean", nullable: false),
                    camera_shake = table.Column<bool>(type: "boolean", nullable: false),
                    score_table = table.Column<bool>(type: "boolean", nullable: false),
                    death_messages = table.Column<bool>(type: "boolean", nullable: false),
                    show_von_id = table.Column<bool>(type: "boolean", nullable: false),
                    show_map_content = table.Column<bool>(type: "boolean", nullable: false),
                    auto_report = table.Column<bool>(type: "boolean", nullable: false),
                    multiple_saves = table.Column<bool>(type: "boolean", nullable: false),
                    ai_skill = table.Column<float>(type: "real", nullable: false),
                    ai_precision = table.Column<float>(type: "real", nullable: false),
                    max_message_send = table.Column<long>(type: "bigint", nullable: false),
                    max_size_guaranteed = table.Column<long>(type: "bigint", nullable: false),
                    max_size_non_guaranteed = table.Column<long>(type: "bigint", nullable: false),
                    min_bandwidth = table.Column<long>(type: "bigint", nullable: false),
                    max_bandwidth = table.Column<long>(type: "bigint", nullable: false),
                    min_error_to_send = table.Column<double>(type: "double precision", nullable: false),
                    min_error_to_send_near = table.Column<double>(type: "double precision", nullable: false),
                    max_custom_file_size = table.Column<long>(type: "bigint", nullable: false),
                    socket_max_packet_size = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    socket_init_bandwidth = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    socket_max_bandwidth = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    terrain_grid = table.Column<long>(type: "bigint", nullable: false),
                    view_distance = table.Column<long>(type: "bigint", nullable: false),
                    enable_hyperthreading = table.Column<bool>(type: "boolean", nullable: false),
                    port = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_game_server_config", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ServiceSlotGroup",
                columns: table => new
                {
                    slot_group_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    mission_id = table.Column<long>(type: "bigint", nullable: false),
                    title = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    description = table.Column<string>(type: "character varying(20000)", maxLength: 20000, nullable: true),
                    order_number = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_service_slot_group", x => x.slot_group_id);
                    table.ForeignKey(
                        name: "fk_service_slot_group_game_server_mission_mission_id",
                        column: x => x.mission_id,
                        principalTable: "GameServerMission",
                        principalColumn: "mission_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ServiceSlot",
                columns: table => new
                {
                    slot_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    slot_group_id = table.Column<long>(type: "bigint", nullable: false),
                    title = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    description = table.Column<string>(type: "character varying(20000)", maxLength: 20000, nullable: true),
                    order_number = table.Column<int>(type: "integer", nullable: false),
                    difficulty = table.Column<int>(type: "integer", nullable: false),
                    reserve = table.Column<bool>(type: "boolean", nullable: false),
                    blocked = table.Column<bool>(type: "boolean", nullable: false),
                    required_dlc = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_service_slot", x => x.slot_id);
                    table.ForeignKey(
                        name: "fk_service_slot_service_slot_group_slot_group_id",
                        column: x => x.slot_group_id,
                        principalTable: "ServiceSlotGroup",
                        principalColumn: "slot_group_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_service_slot_slot_group_id",
                table: "ServiceSlot",
                column: "slot_group_id");

            migrationBuilder.CreateIndex(
                name: "ix_service_slot_group_mission_id",
                table: "ServiceSlotGroup",
                column: "mission_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameServerConfig");

            migrationBuilder.DropTable(
                name: "ServiceSlot");

            migrationBuilder.DropTable(
                name: "ServiceSlotGroup");
        }
    }
}
