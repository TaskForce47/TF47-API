using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace TF47_API.Migrations
{
    public partial class added_squad_xml : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ServiceSquads",
                columns: table => new
                {
                    squad_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    website = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    nick = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    title = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    mail = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    picture_url = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_service_squads", x => x.squad_id);
                });

            migrationBuilder.CreateTable(
                name: "ServiceSquadMembers",
                columns: table => new
                {
                    squad_member_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    remark = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    mail = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    squad_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_service_squad_members", x => x.squad_member_id);
                    table.ForeignKey(
                        name: "fk_service_squad_members_service_squads_squad_id",
                        column: x => x.squad_id,
                        principalTable: "ServiceSquads",
                        principalColumn: "squad_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_service_squad_members_service_users_user_id",
                        column: x => x.user_id,
                        principalTable: "ServiceUsers",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_service_squad_members_squad_id",
                table: "ServiceSquadMembers",
                column: "squad_id");

            migrationBuilder.CreateIndex(
                name: "ix_service_squad_members_user_id",
                table: "ServiceSquadMembers",
                column: "user_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ServiceSquadMembers");

            migrationBuilder.DropTable(
                name: "ServiceSquads");
        }
    }
}
