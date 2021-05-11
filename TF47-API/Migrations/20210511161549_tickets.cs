using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace TF47_API.Migrations
{
    public partial class tickets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "session_finished",
                table: "GameServerSessions",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "GameServerTickets",
                columns: table => new
                {
                    ticket_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    session_id = table.Column<long>(type: "bigint", nullable: false),
                    player_uid1 = table.Column<string>(type: "character varying(100)", nullable: true),
                    player_uid = table.Column<string>(type: "text", nullable: true),
                    text = table.Column<string>(type: "text", nullable: true),
                    new_ticket_count = table.Column<int>(type: "integer", nullable: false),
                    ticket_change_count = table.Column<int>(type: "integer", nullable: false),
                    time_changed = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_game_server_tickets", x => x.ticket_id);
                    table.ForeignKey(
                        name: "fk_game_server_tickets_game_server_players_player_uid1",
                        column: x => x.player_uid1,
                        principalTable: "GameServerPlayers",
                        principalColumn: "player_uid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_game_server_tickets_game_server_sessions_session_id",
                        column: x => x.session_id,
                        principalTable: "GameServerSessions",
                        principalColumn: "session_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_game_server_tickets_player_uid1",
                table: "GameServerTickets",
                column: "player_uid1");

            migrationBuilder.CreateIndex(
                name: "ix_game_server_tickets_session_id",
                table: "GameServerTickets",
                column: "session_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameServerTickets");

            migrationBuilder.DropColumn(
                name: "session_finished",
                table: "GameServerSessions");
        }
    }
}
