using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Fastkart.Migrations
{
    /// <inheritdoc />
    public partial class mergemainn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.CreateTable(
            //    name: "Brand",
            //    columns: table => new
            //    {
            //        Uid = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        BrandName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
            //        Slug = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
            //        Logo = table.Column<string>(type: "nvarchar(max)", unicode: false, nullable: true),
            //        Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
            //        CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETDATE()"),
            //        UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETDATE()"),
            //        CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
            //        UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
            //        Deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Brand", x => x.Uid);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Functions",
            //    columns: table => new
            //    {
            //        Uid = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
            //        Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
            //        Deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
            //        Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, defaultValue: "Active")
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Functions", x => x.Uid);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "PermissionTypes",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
            //        Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_PermissionTypes", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "ProductCategory",
            //    columns: table => new
            //    {
            //        Uid = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        CategoryName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
            //        Thumbnail = table.Column<string>(type: "nvarchar(max)", unicode: false, nullable: true),
            //        Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
            //        Position = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
            //        Slug = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
            //        CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETDATE()"),
            //        UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETDATE()"),
            //        CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
            //        UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
            //        Deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_ProductCategory", x => x.Uid);
            //    });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Uid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETDATE()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Uid);
                });

            migrationBuilder.CreateTable(
                name: "StockStatus",
                columns: table => new
                {
                    Uid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StockName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<bool>(type: "bit", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETDATE()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockStatus", x => x.Uid);
                });

            migrationBuilder.CreateTable(
                name: "Unit",
                columns: table => new
                {
                    Uid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UnitName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Abbreviation = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETDATE()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Unit", x => x.Uid);
                });

            migrationBuilder.CreateTable(
                name: "ProductSubCategory",
                columns: table => new
                {
                    Uid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubCategoryName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CategoryUid = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETDATE()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductSubCategory", x => x.Uid);
                    table.ForeignKey(
                        name: "FK_ProductSubCategory_ProductCategory_CategoryUid",
                        column: x => x.CategoryUid,
                        principalTable: "ProductCategory",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    Uid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    FunctionId = table.Column<int>(type: "int", nullable: false),
                    PermissionTypeId = table.Column<int>(type: "int", nullable: false),
                    Allowed = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.Uid);
                    table.ForeignKey(
                        name: "FK_Permissions_Functions_FunctionId",
                        column: x => x.FunctionId,
                        principalTable: "Functions",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Permissions_PermissionTypes_PermissionTypeId",
                        column: x => x.PermissionTypeId,
                        principalTable: "PermissionTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Permissions_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Uid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ImgUser = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Email = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    PhoneNumber = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoleUid = table.Column<int>(type: "int", nullable: false),
                    OtpCode = table.Column<string>(type: "varchar(6)", unicode: false, maxLength: 6, nullable: true),
                    OtpExpiry = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETDATE()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Uid);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleUid",
                        column: x => x.RoleUid,
                        principalTable: "Roles",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    Uid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    SubCategoryUid = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnitUid = table.Column<int>(type: "int", nullable: false),
                    StockQuantity = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    StockStatusUid = table.Column<int>(type: "int", nullable: false),
                    Sku = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Discount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Thumbnail = table.Column<string>(type: "nvarchar(max)", unicode: false, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Position = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    BrandUid = table.Column<int>(type: "int", nullable: false),
                    Weight = table.Column<double>(type: "float", nullable: false),
                    IsFeatured = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Exchangeable = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    Refundable = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    Slug = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.Uid);
                    table.ForeignKey(
                        name: "FK_Product_Brand_BrandUid",
                        column: x => x.BrandUid,
                        principalTable: "Brand",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Product_ProductSubCategory_SubCategoryUid",
                        column: x => x.SubCategoryUid,
                        principalTable: "ProductSubCategory",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Product_StockStatus_StockStatusUid",
                        column: x => x.StockStatusUid,
                        principalTable: "StockStatus",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Product_Unit_UnitUid",
                        column: x => x.UnitUid,
                        principalTable: "Unit",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Cart",
                columns: table => new
                {
                    Uid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserUid = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cart", x => x.Uid);
                    table.ForeignKey(
                        name: "FK_Cart_Users_UserUid",
                        column: x => x.UserUid,
                        principalTable: "Users",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    Uid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserUid = table.Column<int>(type: "int", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ShippingAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentMethod = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.Uid);
                    table.ForeignKey(
                        name: "FK_Order_Users_UserUid",
                        column: x => x.UserUid,
                        principalTable: "Users",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CartItem",
                columns: table => new
                {
                    Uid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CartUid = table.Column<int>(type: "int", nullable: false),
                    ProductUid = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItem", x => x.Uid);
                    table.ForeignKey(
                        name: "FK_CartItem_Cart_CartUid",
                        column: x => x.CartUid,
                        principalTable: "Cart",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartItem_Product_ProductUid",
                        column: x => x.ProductUid,
                        principalTable: "Product",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderItem",
                columns: table => new
                {
                    Uid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderUid = table.Column<int>(type: "int", nullable: false),
                    ProductUid = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    PriceAtPurchase = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItem", x => x.Uid);
                    table.ForeignKey(
                        name: "FK_OrderItem_Order_OrderUid",
                        column: x => x.OrderUid,
                        principalTable: "Order",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItem_Product_ProductUid",
                        column: x => x.ProductUid,
                        principalTable: "Product",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Payment",
                columns: table => new
                {
                    Uid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderUid = table.Column<int>(type: "int", nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    QrPaymentCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BankTransactionCode = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payment", x => x.Uid);
                    table.ForeignKey(
                        name: "FK_Payment_Order_OrderUid",
                        column: x => x.OrderUid,
                        principalTable: "Order",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Brand",
                columns: new[] { "Uid", "BrandName", "CreatedBy", "Description", "Logo", "Slug", "Status", "UpdatedBy" },
                values: new object[] { 1, "Fresho", null, null, "fresho.png", "fresho", "Active", null });

            migrationBuilder.InsertData(
                table: "ProductCategory",
                columns: new[] { "Uid", "CategoryName", "CreatedBy", "Description", "Slug", "Status", "Thumbnail", "UpdatedBy" },
                values: new object[] { 1, "Rau củ", null, null, "rau-cu", "Active", null, null });

            migrationBuilder.InsertData(
                table: "StockStatus",
                columns: new[] { "Uid", "CreatedBy", "Description", "Status", "StockName", "UpdatedBy" },
                values: new object[] { 1, null, null, true, "Còn hàng", null });

            migrationBuilder.InsertData(
                table: "Unit",
                columns: new[] { "Uid", "Abbreviation", "CreatedBy", "Description", "Status", "UnitName", "UpdatedBy" },
                values: new object[] { 1, "kg", null, null, "Active", "Kilogram", null });

            migrationBuilder.InsertData(
                table: "ProductSubCategory",
                columns: new[] { "Uid", "CategoryUid", "CreatedBy", "Description", "Slug", "Status", "SubCategoryName", "UpdatedBy" },
                values: new object[] { 1, 1, null, "Rau củ tươi", "rau-cu-tuoi", "Active", "Rau củ tươi", null });

            migrationBuilder.InsertData(
                table: "Product",
                columns: new[] { "Uid", "BrandUid", "CreatedBy", "Description", "Discount", "Exchangeable", "IsFeatured", "Price", "ProductName", "Refundable", "Sku", "Slug", "Status", "StockQuantity", "StockStatusUid", "SubCategoryUid", "Thumbnail", "UnitUid", "UpdatedAt", "UpdatedBy", "Weight" },
                values: new object[] { 1, 1, null, null, 0, true, true, 35000m, "Ớt chuông đỏ", true, null, "ot-chuong-do", "Active", 100, 1, 1, "https://themes.pixelstrap.com/fastkart/assets/images/veg-3/cate1/1.png", 1, null, null, 1.0 });

            migrationBuilder.InsertData(
                table: "Product",
                columns: new[] { "Uid", "BrandUid", "CreatedBy", "Description", "Discount", "Exchangeable", "Price", "ProductName", "Refundable", "Sku", "Slug", "Status", "StockQuantity", "StockStatusUid", "SubCategoryUid", "Thumbnail", "UnitUid", "UpdatedAt", "UpdatedBy", "Weight" },
                values: new object[,]
                {
                    { 2, 1, null, null, 0, true, 28000m, "Bông cải xanh", true, null, "bong-cai-xanh", "Active", 100, 1, 1, "https://themes.pixelstrap.com/fastkart/assets/images/veg-3/cate1/6.png", 1, null, null, 1.0 },
                    { 3, 1, null, null, 0, true, 22000m, "Cà rốt hữu cơ", true, null, "ca-rot-huu-co", "Active", 100, 1, 1, "https://themes.pixelstrap.com/fastkart/assets/images/veg-3/pro1/3.png", 1, null, null, 1.0 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cart_UserUid",
                table: "Cart",
                column: "UserUid");

            migrationBuilder.CreateIndex(
                name: "IX_CartItem_CartUid",
                table: "CartItem",
                column: "CartUid");

            migrationBuilder.CreateIndex(
                name: "IX_CartItem_ProductUid",
                table: "CartItem",
                column: "ProductUid");

            migrationBuilder.CreateIndex(
                name: "IX_Order_UserUid",
                table: "Order",
                column: "UserUid");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItem_OrderUid",
                table: "OrderItem",
                column: "OrderUid");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItem_ProductUid",
                table: "OrderItem",
                column: "ProductUid");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_OrderUid",
                table: "Payment",
                column: "OrderUid");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_FunctionId",
                table: "Permissions",
                column: "FunctionId");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_PermissionTypeId",
                table: "Permissions",
                column: "PermissionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_RoleId",
                table: "Permissions",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_BrandUid",
                table: "Product",
                column: "BrandUid");

            migrationBuilder.CreateIndex(
                name: "IX_Product_StockStatusUid",
                table: "Product",
                column: "StockStatusUid");

            migrationBuilder.CreateIndex(
                name: "IX_Product_SubCategoryUid",
                table: "Product",
                column: "SubCategoryUid");

            migrationBuilder.CreateIndex(
                name: "IX_Product_UnitUid",
                table: "Product",
                column: "UnitUid");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSubCategory_CategoryUid",
                table: "ProductSubCategory",
                column: "CategoryUid");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleUid",
                table: "Users",
                column: "RoleUid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartItem");

            migrationBuilder.DropTable(
                name: "OrderItem");

            migrationBuilder.DropTable(
                name: "Payment");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "Cart");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "Functions");

            migrationBuilder.DropTable(
                name: "PermissionTypes");

            migrationBuilder.DropTable(
                name: "Brand");

            migrationBuilder.DropTable(
                name: "ProductSubCategory");

            migrationBuilder.DropTable(
                name: "StockStatus");

            migrationBuilder.DropTable(
                name: "Unit");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "ProductCategory");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
