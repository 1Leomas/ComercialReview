using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Intercon.Infrastructure.Migrations
{
    /// <inheritdoc />
#pragma warning disable CS8981 // The type name only contains lower-cased ascii characters. Such names may become reserved for the language.
    public partial class logorelation : Migration
#pragma warning restore CS8981 // The type name only contains lower-cased ascii characters. Such names may become reserved for the language.
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
