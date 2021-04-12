using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TF47_API.Migrations
{
    public partial class tracking_system : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "game_tick_time",
                table: "GameServerReplayItems",
                type: "real",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<DateTime>(
                name: "time_tracked",
                table: "GameServerReplayItems",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "tracking_id",
                table: "GameServerReplayItems",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "ix_game_server_replay_items_tracking_id",
                table: "GameServerReplayItems",
                column: "tracking_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_game_server_replay_items_tracking_id",
                table: "GameServerReplayItems");

            migrationBuilder.DropColumn(
                name: "time_tracked",
                table: "GameServerReplayItems");

            migrationBuilder.DropColumn(
                name: "tracking_id",
                table: "GameServerReplayItems");

            migrationBuilder.AlterColumn<long>(
                name: "game_tick_time",
                table: "GameServerReplayItems",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");
        }
    }
}
