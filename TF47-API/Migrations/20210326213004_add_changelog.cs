using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace TF47_API.Migrations
{
    public partial class add_changelog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "changelogs",
                columns: table => new
                {
                    changelog_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    tags = table.Column<string[]>(type: "text[]", nullable: true),
                    text = table.Column<string>(type: "character varying(20000)", maxLength: 20000, nullable: true),
                    time_released = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    author_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_changelogs", x => x.changelog_id);
                    table.ForeignKey(
                        name: "fk_changelogs_service_users_author_id",
                        column: x => x.author_id,
                        principalTable: "ServiceUsers",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_changelogs_author_id",
                table: "changelogs",
                column: "author_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "changelogs");
        }
    }
}
