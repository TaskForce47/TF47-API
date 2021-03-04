using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace TF47_Backend.Migrations
{
    public partial class addPasswordResetTokenTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "password_resets",
                columns: table => new
                {
                    password_reset_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    reset_token = table.Column<string>(type: "text", nullable: true),
                    time_password_reset_generated = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_password_resets", x => x.password_reset_id);
                    table.ForeignKey(
                        name: "fk_password_resets_users_user_id",
                        column: x => x.user_id,
                        principalTable: "service_users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_password_resets_user_id",
                table: "password_resets",
                column: "user_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "password_resets");
        }
    }
}
