using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Intercon.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCommentsCountFieldToReview : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CommentsCount",
                table: "Reviews",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CommentsCount",
                table: "Reviews");
        }
    }
}
