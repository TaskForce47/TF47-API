using Microsoft.EntityFrameworkCore.Migrations;

namespace TF47_Backend.Migrations
{
    public partial class updateTablesAndAutoIncrement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_password_resets_users_user_id",
                table: "password_resets");

            migrationBuilder.DropForeignKey(
                name: "fk_user_has_groups_groups_group_id",
                table: "user_has_groups");

            migrationBuilder.DropForeignKey(
                name: "fk_user_has_groups_users_user_id",
                table: "user_has_groups");

            migrationBuilder.DropPrimaryKey(
                name: "pk_user_has_groups",
                table: "user_has_groups");

            migrationBuilder.DropPrimaryKey(
                name: "pk_password_resets",
                table: "password_resets");

            migrationBuilder.RenameTable(
                name: "user_has_groups",
                newName: "service_userhasgroups");

            migrationBuilder.RenameTable(
                name: "password_resets",
                newName: "service_password_resets");

            migrationBuilder.RenameIndex(
                name: "ix_user_has_groups_user_id",
                table: "service_userhasgroups",
                newName: "ix_service_userhasgroups_user_id");

            migrationBuilder.RenameIndex(
                name: "ix_user_has_groups_group_id",
                table: "service_userhasgroups",
                newName: "ix_service_userhasgroups_group_id");

            migrationBuilder.RenameIndex(
                name: "ix_password_resets_user_id",
                table: "service_password_resets",
                newName: "ix_service_password_resets_user_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_service_userhasgroups",
                table: "service_userhasgroups",
                column: "user_has_group_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_service_password_resets",
                table: "service_password_resets",
                column: "password_reset_id");

            migrationBuilder.AddForeignKey(
                name: "fk_service_password_resets_service_users_user_id",
                table: "service_password_resets",
                column: "user_id",
                principalTable: "service_users",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_service_userhasgroups_service_groups_group_id",
                table: "service_userhasgroups",
                column: "group_id",
                principalTable: "service_groups",
                principalColumn: "group_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_service_userhasgroups_service_users_user_id",
                table: "service_userhasgroups",
                column: "user_id",
                principalTable: "service_users",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_service_password_resets_service_users_user_id",
                table: "service_password_resets");

            migrationBuilder.DropForeignKey(
                name: "fk_service_userhasgroups_service_groups_group_id",
                table: "service_userhasgroups");

            migrationBuilder.DropForeignKey(
                name: "fk_service_userhasgroups_service_users_user_id",
                table: "service_userhasgroups");

            migrationBuilder.DropPrimaryKey(
                name: "pk_service_userhasgroups",
                table: "service_userhasgroups");

            migrationBuilder.DropPrimaryKey(
                name: "pk_service_password_resets",
                table: "service_password_resets");

            migrationBuilder.RenameTable(
                name: "service_userhasgroups",
                newName: "user_has_groups");

            migrationBuilder.RenameTable(
                name: "service_password_resets",
                newName: "password_resets");

            migrationBuilder.RenameIndex(
                name: "ix_service_userhasgroups_user_id",
                table: "user_has_groups",
                newName: "ix_user_has_groups_user_id");

            migrationBuilder.RenameIndex(
                name: "ix_service_userhasgroups_group_id",
                table: "user_has_groups",
                newName: "ix_user_has_groups_group_id");

            migrationBuilder.RenameIndex(
                name: "ix_service_password_resets_user_id",
                table: "password_resets",
                newName: "ix_password_resets_user_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_user_has_groups",
                table: "user_has_groups",
                column: "user_has_group_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_password_resets",
                table: "password_resets",
                column: "password_reset_id");

            migrationBuilder.AddForeignKey(
                name: "fk_password_resets_users_user_id",
                table: "password_resets",
                column: "user_id",
                principalTable: "service_users",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_user_has_groups_groups_group_id",
                table: "user_has_groups",
                column: "group_id",
                principalTable: "service_groups",
                principalColumn: "group_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_user_has_groups_users_user_id",
                table: "user_has_groups",
                column: "user_id",
                principalTable: "service_users",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
