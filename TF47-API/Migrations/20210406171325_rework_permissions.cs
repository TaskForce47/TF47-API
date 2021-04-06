using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace TF47_API.Migrations
{
    public partial class rework_permissions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_service_group_permissions_service_groups_group_id",
                table: "ServiceGroupPermissions");

            migrationBuilder.DropPrimaryKey(
                name: "pk_service_group_permissions",
                table: "ServiceGroupPermissions");

            migrationBuilder.DropIndex(
                name: "ix_service_group_permissions_group_id",
                table: "ServiceGroupPermissions");

            migrationBuilder.DropColumn(
                name: "permissions_discord",
                table: "ServiceGroupPermissions");

            migrationBuilder.DropColumn(
                name: "permissions_gadget",
                table: "ServiceGroupPermissions");

            migrationBuilder.DropColumn(
                name: "permissions_teamspeak",
                table: "ServiceGroupPermissions");

            migrationBuilder.RenameColumn(
                name: "group_id",
                table: "ServiceGroupPermissions",
                newName: "permissions_permission_id");

            migrationBuilder.RenameColumn(
                name: "group_permission_id",
                table: "ServiceGroupPermissions",
                newName: "group_permissions_group_id");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:permission_type", "discord,gadget,teamspeak")
                .Annotation("Npgsql:Enum:side", "civilian,bluefor,redfor,independent")
                .Annotation("Npgsql:Enum:vehicle_type", "infantry,light_vehicle,tank,helicopter,fixed_wing,boat")
                .OldAnnotation("Npgsql:Enum:side", "civilian,bluefor,redfor,independent")
                .OldAnnotation("Npgsql:Enum:vehicle_type", "infantry,light_vehicle,tank,helicopter,fixed_wing,boat");

            migrationBuilder.AlterColumn<long>(
                name: "group_permissions_group_id",
                table: "ServiceGroupPermissions",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "pk_service_group_permissions",
                table: "ServiceGroupPermissions",
                columns: new[] { "group_permissions_group_id", "permissions_permission_id" });

            migrationBuilder.CreateTable(
                name: "ServicePermissions",
                columns: table => new
                {
                    permission_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    type = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_service_permissions", x => x.permission_id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_service_group_permissions_permissions_permission_id",
                table: "ServiceGroupPermissions",
                column: "permissions_permission_id");

            migrationBuilder.CreateIndex(
                name: "ix_service_permissions_type",
                table: "ServicePermissions",
                column: "type");

            migrationBuilder.AddForeignKey(
                name: "fk_service_group_permissions_service_groups_group_permissions_gro",
                table: "ServiceGroupPermissions",
                column: "group_permissions_group_id",
                principalTable: "ServiceGroups",
                principalColumn: "group_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_service_group_permissions_service_permissions_permissions_perm",
                table: "ServiceGroupPermissions",
                column: "permissions_permission_id",
                principalTable: "ServicePermissions",
                principalColumn: "permission_id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_service_group_permissions_service_groups_group_permissions_gro",
                table: "ServiceGroupPermissions");

            migrationBuilder.DropForeignKey(
                name: "fk_service_group_permissions_service_permissions_permissions_perm",
                table: "ServiceGroupPermissions");

            migrationBuilder.DropTable(
                name: "ServicePermissions");

            migrationBuilder.DropPrimaryKey(
                name: "pk_service_group_permissions",
                table: "ServiceGroupPermissions");

            migrationBuilder.DropIndex(
                name: "ix_service_group_permissions_permissions_permission_id",
                table: "ServiceGroupPermissions");

            migrationBuilder.RenameColumn(
                name: "permissions_permission_id",
                table: "ServiceGroupPermissions",
                newName: "group_id");

            migrationBuilder.RenameColumn(
                name: "group_permissions_group_id",
                table: "ServiceGroupPermissions",
                newName: "group_permission_id");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:side", "civilian,bluefor,redfor,independent")
                .Annotation("Npgsql:Enum:vehicle_type", "infantry,light_vehicle,tank,helicopter,fixed_wing,boat")
                .OldAnnotation("Npgsql:Enum:permission_type", "discord,gadget,teamspeak")
                .OldAnnotation("Npgsql:Enum:side", "civilian,bluefor,redfor,independent")
                .OldAnnotation("Npgsql:Enum:vehicle_type", "infantry,light_vehicle,tank,helicopter,fixed_wing,boat");

            migrationBuilder.AlterColumn<long>(
                name: "group_permission_id",
                table: "ServiceGroupPermissions",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<string>(
                name: "permissions_discord",
                table: "ServiceGroupPermissions",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "permissions_gadget",
                table: "ServiceGroupPermissions",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "permissions_teamspeak",
                table: "ServiceGroupPermissions",
                type: "text",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "pk_service_group_permissions",
                table: "ServiceGroupPermissions",
                column: "group_permission_id");

            migrationBuilder.CreateIndex(
                name: "ix_service_group_permissions_group_id",
                table: "ServiceGroupPermissions",
                column: "group_id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_service_group_permissions_service_groups_group_id",
                table: "ServiceGroupPermissions",
                column: "group_id",
                principalTable: "ServiceGroups",
                principalColumn: "group_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
