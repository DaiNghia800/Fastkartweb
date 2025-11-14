using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fastkart.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProductThumbnails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Product",
                keyColumn: "Uid",
                keyValue: 1,
                column: "Thumbnail",
                value: "https://themes.pixelstrap.com/fastkart/assets/images/veg-3/cate1/1.png");

            migrationBuilder.UpdateData(
                table: "Product",
                keyColumn: "Uid",
                keyValue: 2,
                column: "Thumbnail",
                value: "https://themes.pixelstrap.com/fastkart/assets/images/veg-3/cate1/6.png");

            migrationBuilder.UpdateData(
                table: "Product",
                keyColumn: "Uid",
                keyValue: 3,
                column: "Thumbnail",
                value: "https://themes.pixelstrap.com/fastkart/assets/images/veg-3/pro1/3.png");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Product",
                keyColumn: "Uid",
                keyValue: 1,
                column: "Thumbnail",
                value: "/images/products/bell-pepper.jpg");

            migrationBuilder.UpdateData(
                table: "Product",
                keyColumn: "Uid",
                keyValue: 2,
                column: "Thumbnail",
                value: "/images/products/broccoli.jpg");

            migrationBuilder.UpdateData(
                table: "Product",
                keyColumn: "Uid",
                keyValue: 3,
                column: "Thumbnail",
                value: "/images/products/carrot.jpg");
        }
    }
}
