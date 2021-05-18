using Microsoft.EntityFrameworkCore.Migrations;

namespace TF47_API.Migrations
{
    public partial class DisableVotingOnGalleryImageFeature : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "can_vote",
                table: "GalleryImages",
                newName: "voting_enabled");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "voting_enabled",
                table: "GalleryImages",
                newName: "can_vote");
        }
    }
}
