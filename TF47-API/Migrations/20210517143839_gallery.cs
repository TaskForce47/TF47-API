using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace TF47_API.Migrations
{
    public partial class gallery : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ServiceGalleries",
                columns: table => new
                {
                    gallery_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    time_created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_service_galleries", x => x.gallery_id);
                });

            migrationBuilder.CreateTable(
                name: "GalleryImages",
                columns: table => new
                {
                    gallery_image_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    time_created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    image_file_name = table.Column<string>(type: "text", nullable: true),
                    gallery_id = table.Column<long>(type: "bigint", nullable: false),
                    can_vote = table.Column<bool>(type: "boolean", nullable: false),
                    uploader_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_gallery_images", x => x.gallery_image_id);
                    table.ForeignKey(
                        name: "fk_gallery_images_service_galleries_gallery_id",
                        column: x => x.gallery_id,
                        principalTable: "ServiceGalleries",
                        principalColumn: "gallery_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_gallery_images_service_users_uploader_id",
                        column: x => x.uploader_id,
                        principalTable: "ServiceUsers",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GalleryImageComments",
                columns: table => new
                {
                    gallery_image_comment_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    comment = table.Column<string>(type: "text", nullable: true),
                    is_edited = table.Column<bool>(type: "boolean", nullable: false),
                    time_last_edited = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    gallery_image_id = table.Column<long>(type: "bigint", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    time_created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_gallery_image_comments", x => x.gallery_image_comment_id);
                    table.ForeignKey(
                        name: "fk_gallery_image_comments_gallery_images_gallery_image_id",
                        column: x => x.gallery_image_id,
                        principalTable: "GalleryImages",
                        principalColumn: "gallery_image_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_gallery_image_comments_service_users_user_id",
                        column: x => x.user_id,
                        principalTable: "ServiceUsers",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ServiceGalleryImageUserDownVotes",
                columns: table => new
                {
                    down_votes_user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    gallery_image_down_votes_gallery_image_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_service_gallery_image_user_down_votes", x => new { x.down_votes_user_id, x.gallery_image_down_votes_gallery_image_id });
                    table.ForeignKey(
                        name: "fk_service_gallery_image_user_down_votes_gallery_images_gallery_imag",
                        column: x => x.gallery_image_down_votes_gallery_image_id,
                        principalTable: "GalleryImages",
                        principalColumn: "gallery_image_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_service_gallery_image_user_down_votes_service_users_down_votes_us",
                        column: x => x.down_votes_user_id,
                        principalTable: "ServiceUsers",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ServiceGalleryImageUserUpVotes",
                columns: table => new
                {
                    gallery_image_up_votes_gallery_image_id = table.Column<long>(type: "bigint", nullable: false),
                    up_votes_user_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_service_gallery_image_user_up_votes", x => new { x.gallery_image_up_votes_gallery_image_id, x.up_votes_user_id });
                    table.ForeignKey(
                        name: "fk_service_gallery_image_user_up_votes_gallery_images_gallery_image_",
                        column: x => x.gallery_image_up_votes_gallery_image_id,
                        principalTable: "GalleryImages",
                        principalColumn: "gallery_image_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_service_gallery_image_user_up_votes_service_users_up_votes_user_id",
                        column: x => x.up_votes_user_id,
                        principalTable: "ServiceUsers",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_gallery_image_comments_gallery_image_id",
                table: "GalleryImageComments",
                column: "gallery_image_id");

            migrationBuilder.CreateIndex(
                name: "ix_gallery_image_comments_user_id",
                table: "GalleryImageComments",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_gallery_images_gallery_id",
                table: "GalleryImages",
                column: "gallery_id");

            migrationBuilder.CreateIndex(
                name: "ix_gallery_images_uploader_id",
                table: "GalleryImages",
                column: "uploader_id");

            migrationBuilder.CreateIndex(
                name: "ix_service_gallery_image_user_down_votes_gallery_image_down_votes_g",
                table: "ServiceGalleryImageUserDownVotes",
                column: "gallery_image_down_votes_gallery_image_id");

            migrationBuilder.CreateIndex(
                name: "ix_service_gallery_image_user_up_votes_up_votes_user_id",
                table: "ServiceGalleryImageUserUpVotes",
                column: "up_votes_user_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GalleryImageComments");

            migrationBuilder.DropTable(
                name: "ServiceGalleryImageUserDownVotes");

            migrationBuilder.DropTable(
                name: "ServiceGalleryImageUserUpVotes");

            migrationBuilder.DropTable(
                name: "GalleryImages");

            migrationBuilder.DropTable(
                name: "ServiceGalleries");
        }
    }
}
