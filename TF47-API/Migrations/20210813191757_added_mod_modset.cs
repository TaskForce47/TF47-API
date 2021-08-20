using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TF47_API.Migrations
{
    public partial class added_mod_modset : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "id",
                table: "GameServerConfig",
                newName: "server_config_id");

            migrationBuilder.AddColumn<long>(
                name: "modset_id",
                table: "GameServerMission",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "modset_id",
                table: "GameServer",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "server_config_id",
                table: "GameServer",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "GameServerMods",
                columns: table => new
                {
                    mod_id = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    slug = table.Column<string>(type: "text", nullable: true),
                    has_key = table.Column<bool>(type: "boolean", nullable: false),
                    file_size = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    time_last_update = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    time_installed = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    mod_status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_game_server_mods", x => x.mod_id);
                });

            migrationBuilder.CreateTable(
                name: "GameServerModsets",
                columns: table => new
                {
                    modset_id = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_game_server_modsets", x => x.modset_id);
                });

            migrationBuilder.CreateTable(
                name: "GameServerModsetMods",
                columns: table => new
                {
                    modesets_modset_id = table.Column<long>(type: "bigint", nullable: false),
                    mods_mod_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_game_server_modset_mods", x => new { x.modesets_modset_id, x.mods_mod_id });
                    table.ForeignKey(
                        name: "fk_game_server_modset_mods_game_server_mods_mods_mod_id",
                        column: x => x.mods_mod_id,
                        principalTable: "GameServerMods",
                        principalColumn: "mod_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_game_server_modset_mods_game_server_modsets_modesets_modset_id",
                        column: x => x.modesets_modset_id,
                        principalTable: "GameServerModsets",
                        principalColumn: "modset_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_game_server_mission_modset_id",
                table: "GameServerMission",
                column: "modset_id");

            migrationBuilder.CreateIndex(
                name: "ix_game_server_modset_id",
                table: "GameServer",
                column: "modset_id");

            migrationBuilder.CreateIndex(
                name: "ix_game_server_server_config_id",
                table: "GameServer",
                column: "server_config_id");

            migrationBuilder.CreateIndex(
                name: "ix_game_server_modset_mods_mods_mod_id",
                table: "GameServerModsetMods",
                column: "mods_mod_id");

            migrationBuilder.AddForeignKey(
                name: "fk_game_server_game_server_config_server_configuration_server_con",
                table: "GameServer",
                column: "server_config_id",
                principalTable: "GameServerConfig",
                principalColumn: "server_config_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_game_server_game_server_modsets_modset_id",
                table: "GameServer",
                column: "modset_id",
                principalTable: "GameServerModsets",
                principalColumn: "modset_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_game_server_mission_game_server_modsets_modset_id",
                table: "GameServerMission",
                column: "modset_id",
                principalTable: "GameServerModsets",
                principalColumn: "modset_id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_game_server_game_server_config_server_configuration_server_con",
                table: "GameServer");

            migrationBuilder.DropForeignKey(
                name: "fk_game_server_game_server_modsets_modset_id",
                table: "GameServer");

            migrationBuilder.DropForeignKey(
                name: "fk_game_server_mission_game_server_modsets_modset_id",
                table: "GameServerMission");

            migrationBuilder.DropTable(
                name: "GameServerModsetMods");

            migrationBuilder.DropTable(
                name: "GameServerMods");

            migrationBuilder.DropTable(
                name: "GameServerModsets");

            migrationBuilder.DropIndex(
                name: "ix_game_server_mission_modset_id",
                table: "GameServerMission");

            migrationBuilder.DropIndex(
                name: "ix_game_server_modset_id",
                table: "GameServer");

            migrationBuilder.DropIndex(
                name: "ix_game_server_server_config_id",
                table: "GameServer");

            migrationBuilder.DropColumn(
                name: "modset_id",
                table: "GameServerMission");

            migrationBuilder.DropColumn(
                name: "modset_id",
                table: "GameServer");

            migrationBuilder.DropColumn(
                name: "server_config_id",
                table: "GameServer");

            migrationBuilder.RenameColumn(
                name: "server_config_id",
                table: "GameServerConfig",
                newName: "id");
        }
    }
}
