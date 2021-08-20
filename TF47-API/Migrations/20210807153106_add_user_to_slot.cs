using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TF47_API.Migrations
{
    public partial class add_user_to_slot : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_service_slot_service_slot_group_slot_group_id",
                table: "ServiceSlot");

            migrationBuilder.AddColumn<Guid>(
                name: "user_id",
                table: "ServiceSlot",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "ix_service_slot_user_id",
                table: "ServiceSlot",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "fk_service_slot_service_slot_group_slot_group_id",
                table: "ServiceSlot",
                column: "slot_group_id",
                principalTable: "ServiceSlotGroup",
                principalColumn: "slot_group_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_service_slot_service_users_user_id",
                table: "ServiceSlot",
                column: "user_id",
                principalTable: "ServiceUsers",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_service_slot_service_slot_group_slot_group_id",
                table: "ServiceSlot");

            migrationBuilder.DropForeignKey(
                name: "fk_service_slot_service_users_user_id",
                table: "ServiceSlot");

            migrationBuilder.DropIndex(
                name: "ix_service_slot_user_id",
                table: "ServiceSlot");

            migrationBuilder.DropColumn(
                name: "user_id",
                table: "ServiceSlot");

            migrationBuilder.AddForeignKey(
                name: "fk_service_slot_service_slot_group_slot_group_id",
                table: "ServiceSlot",
                column: "slot_group_id",
                principalTable: "ServiceSlotGroup",
                principalColumn: "slot_group_id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
