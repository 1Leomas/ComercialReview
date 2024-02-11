using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Intercon.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class logorelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Businesses_Images_LogoId",
                table: "Businesses");

            migrationBuilder.DropIndex(
                name: "IX_Businesses_LogoId",
                table: "Businesses");

            migrationBuilder.CreateIndex(
                name: "IX_Businesses_LogoId",
                table: "Businesses",
                column: "LogoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Businesses_Images_LogoId",
                table: "Businesses",
                column: "LogoId",
                principalTable: "Images",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Businesses_Images_LogoId",
                table: "Businesses");

            migrationBuilder.DropIndex(
                name: "IX_Businesses_LogoId",
                table: "Businesses");

            migrationBuilder.CreateIndex(
                name: "IX_Businesses_LogoId",
                table: "Businesses",
                column: "LogoId",
                unique: true,
                filter: "[LogoId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Businesses_Images_LogoId",
                table: "Businesses",
                column: "LogoId",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
