using Microsoft.EntityFrameworkCore.Migrations;

namespace TF47_API.Migrations
{
    public partial class update_playtimes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "session_ended",
                table: "GameServerSessions",
                newName: "time_ended");

            migrationBuilder.RenameColumn(
                name: "session_created",
                table: "GameServerSessions",
                newName: "time_created");

            migrationBuilder.RenameColumn(
                name: "last_visit",
                table: "GameServerPlayers",
                newName: "time_last_visit");

            migrationBuilder.RenameColumn(
                name: "first_visit",
                table: "GameServerPlayers",
                newName: "time_first_visit");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "time_ended",
                table: "GameServerSessions",
                newName: "session_ended");

            migrationBuilder.RenameColumn(
                name: "time_created",
                table: "GameServerSessions",
                newName: "session_created");

            migrationBuilder.RenameColumn(
                name: "time_last_visit",
                table: "GameServerPlayers",
                newName: "last_visit");

            migrationBuilder.RenameColumn(
                name: "time_first_visit",
                table: "GameServerPlayers",
                newName: "first_visit");
        }
    }
}
