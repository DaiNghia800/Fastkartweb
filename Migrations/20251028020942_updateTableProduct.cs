using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fastkart.Migrations
{
    /// <inheritdoc />
    public partial class updateTableProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_ProductCategory_CategoryUid",
                table: "Product");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductSubCategory_ProductCategory_CategoryUid",
                table: "ProductSubCategory");

            migrationBuilder.DropIndex(
                name: "IX_Product_CategoryUid",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "CategoryUid",
                table: "Product");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSubCategory_ProductCategory_CategoryUid",
                table: "ProductSubCategory",
                column: "CategoryUid",
                principalTable: "ProductCategory",
                principalColumn: "Uid",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductSubCategory_ProductCategory_CategoryUid",
                table: "ProductSubCategory");

            migrationBuilder.AddColumn<int>(
                name: "CategoryUid",
                table: "Product",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Product_CategoryUid",
                table: "Product",
                column: "CategoryUid");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_ProductCategory_CategoryUid",
                table: "Product",
                column: "CategoryUid",
                principalTable: "ProductCategory",
                principalColumn: "Uid",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSubCategory_ProductCategory_CategoryUid",
                table: "ProductSubCategory",
                column: "CategoryUid",
                principalTable: "ProductCategory",
                principalColumn: "Uid",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
