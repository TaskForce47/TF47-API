using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TF47_API.Migrations
{
    public partial class add_user_to_slot3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_service_slot_service_users_user_id",
                table: "ServiceSlot");

            migrationBuilder.AlterColumn<Guid>(
                name: "user_id",
                table: "ServiceSlot",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "fk_service_slot_service_users_user_id",
                table: "ServiceSlot",
                column: "user_id",
                principalTable: "ServiceUsers",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_service_slot_service_users_user_id",
                table: "ServiceSlot");

            migrationBuilder.AlterColumn<Guid>(
                name: "user_id",
                table: "ServiceSlot",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "fk_service_slot_service_users_user_id",
                table: "ServiceSlot",
                column: "user_id",
                principalTable: "ServiceUsers",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
