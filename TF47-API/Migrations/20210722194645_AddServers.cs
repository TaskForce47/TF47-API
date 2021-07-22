using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace TF47_API.Migrations
{
    public partial class AddServers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:advanced_flight_model_setting", "up_to_player,force_simple_flight_model,force_advanced_flight_model")
                .Annotation("Npgsql:Enum:difficulty_setting", "recruit,regular,veteran,custom")
                .Annotation("Npgsql:Enum:game_server_status", "running,restarting,updating,stopped,crashed")
                .Annotation("Npgsql:Enum:mission_type", "coop,tv_t,cti")
                .Annotation("Npgsql:Enum:mod_status", "installed,waiting_for_update,updating")
                .Annotation("Npgsql:Enum:never_distance_or_fadeout_setting", "never,limited_by_distance,always")
                .Annotation("Npgsql:Enum:never_fade_out_always_setting", "never,fade_out,always")
                .Annotation("Npgsql:Enum:permission_type", "discord,gadget,teamspeak")
                .Annotation("Npgsql:Enum:side", "civilian,bluefor,redfor,independent")
                .Annotation("Npgsql:Enum:vehicle_type", "infantry,light_vehicle,tank,helicopter,fixed_wing,boat")
                .Annotation("Npgsql:Enum:von_codec_setting", "speex_codec,opus_codec")
                .OldAnnotation("Npgsql:Enum:permission_type", "discord,gadget,teamspeak")
                .OldAnnotation("Npgsql:Enum:side", "civilian,bluefor,redfor,independent")
                .OldAnnotation("Npgsql:Enum:vehicle_type", "infantry,light_vehicle,tank,helicopter,fixed_wing,boat");

            migrationBuilder.CreateTable(
                name: "GameServer",
                columns: table => new
                {
                    server_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    ip = table.Column<string>(type: "text", nullable: false),
                    port = table.Column<string>(type: "text", nullable: false),
                    branch = table.Column<string>(type: "text", nullable: true),
                    last_time_started = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    game_server_status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_game_server", x => x.server_id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameServer");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:permission_type", "discord,gadget,teamspeak")
                .Annotation("Npgsql:Enum:side", "civilian,bluefor,redfor,independent")
                .Annotation("Npgsql:Enum:vehicle_type", "infantry,light_vehicle,tank,helicopter,fixed_wing,boat")
                .OldAnnotation("Npgsql:Enum:advanced_flight_model_setting", "up_to_player,force_simple_flight_model,force_advanced_flight_model")
                .OldAnnotation("Npgsql:Enum:difficulty_setting", "recruit,regular,veteran,custom")
                .OldAnnotation("Npgsql:Enum:game_server_status", "running,restarting,updating,stopped,crashed")
                .OldAnnotation("Npgsql:Enum:mission_type", "coop,tv_t,cti")
                .OldAnnotation("Npgsql:Enum:mod_status", "installed,waiting_for_update,updating")
                .OldAnnotation("Npgsql:Enum:never_distance_or_fadeout_setting", "never,limited_by_distance,always")
                .OldAnnotation("Npgsql:Enum:never_fade_out_always_setting", "never,fade_out,always")
                .OldAnnotation("Npgsql:Enum:permission_type", "discord,gadget,teamspeak")
                .OldAnnotation("Npgsql:Enum:side", "civilian,bluefor,redfor,independent")
                .OldAnnotation("Npgsql:Enum:vehicle_type", "infantry,light_vehicle,tank,helicopter,fixed_wing,boat")
                .OldAnnotation("Npgsql:Enum:von_codec_setting", "speex_codec,opus_codec");
        }
    }
}
