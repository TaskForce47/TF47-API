using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace TF47_Backend.Migrations
{
    public partial class issuetags : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_service_issueitems_service_issues_issue_id",
                table: "service_issueitems");

            migrationBuilder.DropForeignKey(
                name: "fk_service_issueitems_service_users_author_user_id",
                table: "service_issueitems");

            migrationBuilder.DropForeignKey(
                name: "fk_service_issues_service_issuegroups_issue_group_id",
                table: "service_issues");

            migrationBuilder.DropPrimaryKey(
                name: "pk_service_issueitems",
                table: "service_issueitems");

            migrationBuilder.DropPrimaryKey(
                name: "pk_service_issuegroups",
                table: "service_issuegroups");

            migrationBuilder.DropColumn(
                name: "tags",
                table: "service_issues");

            migrationBuilder.RenameTable(
                name: "service_issueitems",
                newName: "service_issue_items");

            migrationBuilder.RenameTable(
                name: "service_issuegroups",
                newName: "service_issue_groups");

            migrationBuilder.RenameIndex(
                name: "ix_service_issueitems_issue_id",
                table: "service_issue_items",
                newName: "ix_service_issue_items_issue_id");

            migrationBuilder.RenameIndex(
                name: "ix_service_issueitems_author_user_id",
                table: "service_issue_items",
                newName: "ix_service_issue_items_author_user_id");

            migrationBuilder.RenameIndex(
                name: "ix_service_issuegroups_group_name",
                table: "service_issue_groups",
                newName: "ix_service_issue_groups_group_name");

            migrationBuilder.AddPrimaryKey(
                name: "pk_service_issue_items",
                table: "service_issue_items",
                column: "issue_item_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_service_issue_groups",
                table: "service_issue_groups",
                column: "issue_group_id");

            migrationBuilder.CreateTable(
                name: "service_issue_tags",
                columns: table => new
                {
                    issue_tag_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    tag_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    color = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_service_issue_tags", x => x.issue_tag_id);
                });

            migrationBuilder.CreateTable(
                name: "service_issue_has_tags",
                columns: table => new
                {
                    issue_tags_issue_tag_id = table.Column<long>(type: "bigint", nullable: false),
                    issues_issue_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_service_issue_has_tags", x => new { x.issue_tags_issue_tag_id, x.issues_issue_id });
                    table.ForeignKey(
                        name: "fk_service_issue_has_tags_issue_tags_issue_tags_issue_tag_id",
                        column: x => x.issue_tags_issue_tag_id,
                        principalTable: "service_issue_tags",
                        principalColumn: "issue_tag_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_service_issue_has_tags_service_issues_issues_issue_id",
                        column: x => x.issues_issue_id,
                        principalTable: "service_issues",
                        principalColumn: "issue_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_service_issue_has_tags_issues_issue_id",
                table: "service_issue_has_tags",
                column: "issues_issue_id");

            migrationBuilder.CreateIndex(
                name: "ix_service_issue_tags_tag_name",
                table: "service_issue_tags",
                column: "tag_name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_service_issue_items_service_issues_issue_id",
                table: "service_issue_items",
                column: "issue_id",
                principalTable: "service_issues",
                principalColumn: "issue_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_service_issue_items_service_users_author_user_id",
                table: "service_issue_items",
                column: "author_user_id",
                principalTable: "service_users",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_service_issues_service_issue_groups_issue_group_id",
                table: "service_issues",
                column: "issue_group_id",
                principalTable: "service_issue_groups",
                principalColumn: "issue_group_id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_service_issue_items_service_issues_issue_id",
                table: "service_issue_items");

            migrationBuilder.DropForeignKey(
                name: "fk_service_issue_items_service_users_author_user_id",
                table: "service_issue_items");

            migrationBuilder.DropForeignKey(
                name: "fk_service_issues_service_issue_groups_issue_group_id",
                table: "service_issues");

            migrationBuilder.DropTable(
                name: "service_issue_has_tags");

            migrationBuilder.DropTable(
                name: "service_issue_tags");

            migrationBuilder.DropPrimaryKey(
                name: "pk_service_issue_items",
                table: "service_issue_items");

            migrationBuilder.DropPrimaryKey(
                name: "pk_service_issue_groups",
                table: "service_issue_groups");

            migrationBuilder.RenameTable(
                name: "service_issue_items",
                newName: "service_issueitems");

            migrationBuilder.RenameTable(
                name: "service_issue_groups",
                newName: "service_issuegroups");

            migrationBuilder.RenameIndex(
                name: "ix_service_issue_items_issue_id",
                table: "service_issueitems",
                newName: "ix_service_issueitems_issue_id");

            migrationBuilder.RenameIndex(
                name: "ix_service_issue_items_author_user_id",
                table: "service_issueitems",
                newName: "ix_service_issueitems_author_user_id");

            migrationBuilder.RenameIndex(
                name: "ix_service_issue_groups_group_name",
                table: "service_issuegroups",
                newName: "ix_service_issuegroups_group_name");

            migrationBuilder.AddColumn<string[]>(
                name: "tags",
                table: "service_issues",
                type: "text[]",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "pk_service_issueitems",
                table: "service_issueitems",
                column: "issue_item_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_service_issuegroups",
                table: "service_issuegroups",
                column: "issue_group_id");

            migrationBuilder.AddForeignKey(
                name: "fk_service_issueitems_service_issues_issue_id",
                table: "service_issueitems",
                column: "issue_id",
                principalTable: "service_issues",
                principalColumn: "issue_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_service_issueitems_service_users_author_user_id",
                table: "service_issueitems",
                column: "author_user_id",
                principalTable: "service_users",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_service_issues_service_issuegroups_issue_group_id",
                table: "service_issues",
                column: "issue_group_id",
                principalTable: "service_issuegroups",
                principalColumn: "issue_group_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
