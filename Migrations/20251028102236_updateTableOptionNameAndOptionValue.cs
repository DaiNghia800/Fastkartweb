using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fastkart.Migrations
{
    /// <inheritdoc />
    public partial class updateTableOptionNameAndOptionValue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OptionValue_OptionName_OptionNameId",
                table: "OptionValue");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductVariantOptionValue_OptionValue_OptionValueId",
                table: "ProductVariantOptionValue");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductVariantOptionValue_ProductVariant_ProductVariantId",
                table: "ProductVariantOptionValue");

            migrationBuilder.RenameColumn(
                name: "ProductVariantId",
                table: "ProductVariantOptionValue",
                newName: "ProductVariantUid");

            migrationBuilder.RenameColumn(
                name: "OptionValueId",
                table: "ProductVariantOptionValue",
                newName: "OptionValueUid");

            migrationBuilder.RenameIndex(
                name: "IX_ProductVariantOptionValue_ProductVariantId_OptionValueId",
                table: "ProductVariantOptionValue",
                newName: "IX_ProductVariantOptionValue_ProductVariantUid_OptionValueUid");

            migrationBuilder.RenameIndex(
                name: "IX_ProductVariantOptionValue_OptionValueId",
                table: "ProductVariantOptionValue",
                newName: "IX_ProductVariantOptionValue_OptionValueUid");

            migrationBuilder.RenameColumn(
                name: "OptionNameId",
                table: "OptionValue",
                newName: "OptionNameUid");

            migrationBuilder.RenameIndex(
                name: "IX_OptionValue_OptionNameId_Value",
                table: "OptionValue",
                newName: "IX_OptionValue_OptionNameUid_Value");

            migrationBuilder.AddForeignKey(
                name: "FK_OptionValue_OptionName_OptionNameUid",
                table: "OptionValue",
                column: "OptionNameUid",
                principalTable: "OptionName",
                principalColumn: "Uid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductVariantOptionValue_OptionValue_OptionValueUid",
                table: "ProductVariantOptionValue",
                column: "OptionValueUid",
                principalTable: "OptionValue",
                principalColumn: "Uid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductVariantOptionValue_ProductVariant_ProductVariantUid",
                table: "ProductVariantOptionValue",
                column: "ProductVariantUid",
                principalTable: "ProductVariant",
                principalColumn: "Uid",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OptionValue_OptionName_OptionNameUid",
                table: "OptionValue");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductVariantOptionValue_OptionValue_OptionValueUid",
                table: "ProductVariantOptionValue");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductVariantOptionValue_ProductVariant_ProductVariantUid",
                table: "ProductVariantOptionValue");

            migrationBuilder.RenameColumn(
                name: "ProductVariantUid",
                table: "ProductVariantOptionValue",
                newName: "ProductVariantId");

            migrationBuilder.RenameColumn(
                name: "OptionValueUid",
                table: "ProductVariantOptionValue",
                newName: "OptionValueId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductVariantOptionValue_ProductVariantUid_OptionValueUid",
                table: "ProductVariantOptionValue",
                newName: "IX_ProductVariantOptionValue_ProductVariantId_OptionValueId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductVariantOptionValue_OptionValueUid",
                table: "ProductVariantOptionValue",
                newName: "IX_ProductVariantOptionValue_OptionValueId");

            migrationBuilder.RenameColumn(
                name: "OptionNameUid",
                table: "OptionValue",
                newName: "OptionNameId");

            migrationBuilder.RenameIndex(
                name: "IX_OptionValue_OptionNameUid_Value",
                table: "OptionValue",
                newName: "IX_OptionValue_OptionNameId_Value");

            migrationBuilder.AddForeignKey(
                name: "FK_OptionValue_OptionName_OptionNameId",
                table: "OptionValue",
                column: "OptionNameId",
                principalTable: "OptionName",
                principalColumn: "Uid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductVariantOptionValue_OptionValue_OptionValueId",
                table: "ProductVariantOptionValue",
                column: "OptionValueId",
                principalTable: "OptionValue",
                principalColumn: "Uid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductVariantOptionValue_ProductVariant_ProductVariantId",
                table: "ProductVariantOptionValue",
                column: "ProductVariantId",
                principalTable: "ProductVariant",
                principalColumn: "Uid",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
