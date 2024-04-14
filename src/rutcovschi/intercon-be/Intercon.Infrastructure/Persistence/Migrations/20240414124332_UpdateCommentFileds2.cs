using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Intercon.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCommentFileds2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Reviews_BusinessId_ReviewId",
                table: "Comments");

            migrationBuilder.RenameColumn(
                name: "ReviewId",
                table: "Comments",
                newName: "ReviewAuthorId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_BusinessId_ReviewId",
                table: "Comments",
                newName: "IX_Comments_BusinessId_ReviewAuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Reviews_BusinessId_ReviewAuthorId",
                table: "Comments",
                columns: new[] { "BusinessId", "ReviewAuthorId" },
                principalTable: "Reviews",
                principalColumns: new[] { "BusinessId", "AuthorId" },
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Reviews_BusinessId_ReviewAuthorId",
                table: "Comments");

            migrationBuilder.RenameColumn(
                name: "ReviewAuthorId",
                table: "Comments",
                newName: "ReviewId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_BusinessId_ReviewAuthorId",
                table: "Comments",
                newName: "IX_Comments_BusinessId_ReviewId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Reviews_BusinessId_ReviewId",
                table: "Comments",
                columns: new[] { "BusinessId", "ReviewId" },
                principalTable: "Reviews",
                principalColumns: new[] { "BusinessId", "AuthorId" },
                onDelete: ReferentialAction.Cascade);
        }
    }
}
