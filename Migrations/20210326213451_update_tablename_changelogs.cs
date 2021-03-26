using Microsoft.EntityFrameworkCore.Migrations;

namespace TF47_Backend.Migrations
{
    public partial class update_tablename_changelogs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_changelogs_service_users_author_id",
                table: "changelogs");

            migrationBuilder.DropPrimaryKey(
                name: "pk_changelogs",
                table: "changelogs");

            migrationBuilder.RenameTable(
                name: "changelogs",
                newName: "ServiceChangelogs");

            migrationBuilder.RenameIndex(
                name: "ix_changelogs_author_id",
                table: "ServiceChangelogs",
                newName: "ix_service_changelogs_author_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_service_changelogs",
                table: "ServiceChangelogs",
                column: "changelog_id");

            migrationBuilder.AddForeignKey(
                name: "fk_service_changelogs_service_users_author_id",
                table: "ServiceChangelogs",
                column: "author_id",
                principalTable: "ServiceUsers",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_service_changelogs_service_users_author_id",
                table: "ServiceChangelogs");

            migrationBuilder.DropPrimaryKey(
                name: "pk_service_changelogs",
                table: "ServiceChangelogs");

            migrationBuilder.RenameTable(
                name: "ServiceChangelogs",
                newName: "changelogs");

            migrationBuilder.RenameIndex(
                name: "ix_service_changelogs_author_id",
                table: "changelogs",
                newName: "ix_changelogs_author_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_changelogs",
                table: "changelogs",
                column: "changelog_id");

            migrationBuilder.AddForeignKey(
                name: "fk_changelogs_service_users_author_id",
                table: "changelogs",
                column: "author_id",
                principalTable: "ServiceUsers",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
