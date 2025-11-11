using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fastkart.Migrations
{
    /// <inheritdoc />
    public partial class updateTableProductCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Icon",
                table: "ProductCategory");

            migrationBuilder.AddColumn<int>(
                name: "Position",
                table: "ProductCategory",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Position",
                table: "ProductCategory");

            migrationBuilder.AddColumn<string>(
                name: "Icon",
                table: "ProductCategory",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);
        }
    }
}
