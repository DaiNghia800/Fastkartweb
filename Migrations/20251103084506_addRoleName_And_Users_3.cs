using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fastkart.Migrations
{
    /// <inheritdoc />
    public partial class addRoleName_And_Users_3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImgUser",
                table: "Users",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImgUser",
                table: "Users");
        }
    }
}
