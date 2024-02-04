using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Intercon.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class setlogonotrequired : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Businesses_Image_LogoId",
                table: "Businesses");

            migrationBuilder.AlterColumn<int>(
                name: "LogoId",
                table: "Businesses",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "FullDescription",
                table: "Businesses",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddForeignKey(
                name: "FK_Businesses_Image_LogoId",
                table: "Businesses",
                column: "LogoId",
                principalTable: "Image",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Businesses_Image_LogoId",
                table: "Businesses");

            migrationBuilder.AlterColumn<int>(
                name: "LogoId",
                table: "Businesses",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FullDescription",
                table: "Businesses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Businesses_Image_LogoId",
                table: "Businesses",
                column: "LogoId",
                principalTable: "Image",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
