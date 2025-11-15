using CloudinaryDotNet.Actions;
using Fastkart.Models.Entities;
using Microsoft.EntityFrameworkCore;

// Đảm bảo namespace này khớp với project của bạn
namespace Fastkart.Models.EF 
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        // --- CÁC BẢNG SẢN PHẨM ---
        public DbSet<ProductCategory> ProductCategory { get; set; }
        public DbSet<ProductSubCategory> ProductSubCategory { get; set; }
        public DbSet<Brand> Brand { get; set; }
        public DbSet<Unit> Unit { get; set; }
        public DbSet<StockStatus> StockStatus { get; set; }
        public DbSet<Product> Product { get; set; }

        // --- CÁC BẢNG USER/PHÂN QUYỀN ---
        public DbSet<Roles> Roles { get; set; }
        public DbSet<Function> Functions { get; set; }
        public DbSet<PermissionType> PermissionTypes { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Users> Users { get; set; }

        // --- CÁC BẢNG E-COMMERCE (THÊM VÀO) ---
        public DbSet<Cart> Cart { get; set; }
        public DbSet<CartItem> CartItem { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderItem> OrderItem { get; set; }
        public DbSet<Payment> Payment { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // --- CẤU HÌNH & SEED DỮ LIỆU ---

            modelBuilder.Entity<ProductCategory>(entity =>
            {
                entity.HasKey(e => e.Uid);
                entity.Property(e => e.CategoryName)
                    .HasMaxLength(255)
                    .IsRequired();
                entity.Property(e => e.Thumbnail)
                    .HasColumnType("nvarchar(max)")
                    .IsUnicode(false)
                    .IsRequired(false);
                entity.Property(e => e.Description)
                    .HasColumnType("nvarchar(max)")
                    .IsRequired(false);
                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .IsRequired(true);
                entity.Property(e => e.Position)
                    .HasDefaultValue(0)
                    .IsRequired();
                entity.Property(e => e.Slug)
                    .HasMaxLength(255)
                    .IsRequired(true);
                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(100)
                    .IsUnicode(true)
                    .IsRequired(false);
                entity.Property(e => e.UpdatedBy)
                    .HasMaxLength(100)
                    .IsUnicode(true)
                    .IsRequired(false);
                entity.Property(e => e.Deleted)
                    .HasDefaultValue(false);
                
                // SEED
                entity.HasData(
                    new ProductCategory { Uid = 1, CategoryName = "Rau củ", Slug = "rau-cu", Status = "Active" }
                );
            });

            modelBuilder.Entity<ProductSubCategory>(entity =>
            {
                entity.HasKey(e => e.Uid);
                entity.Property(e => e.SubCategoryName)
                    .HasMaxLength(255)
                    .IsRequired();
                entity.Property(e => e.Slug)
                    .HasMaxLength(255)
                    .IsRequired();
                entity.HasOne(e => e.ProductCategory)
                    .WithMany(p => p.SubCategories)
                    .HasForeignKey(e => e.CategoryUid)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.Description)
                    .HasColumnType("nvarchar(max)")
                    .IsRequired(true);
                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .IsRequired();
                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(100)
                    .IsUnicode(true);
                entity.Property(e => e.UpdatedBy)
                    .HasMaxLength(100)
                    .IsUnicode(true);
                entity.Property(e => e.Deleted)
                    .HasDefaultValue(false);
                
                // SEED
                entity.HasData(
                    new ProductSubCategory { Uid = 1, CategoryUid = 1, SubCategoryName = "Rau củ tươi", Slug = "rau-cu-tuoi", Description = "Rau củ tươi", Status = "Active" }
                );
            });

            modelBuilder.Entity<Brand>(entity =>
            {
                entity.HasKey(e => e.Uid);
                entity.Property(e => e.BrandName)
                    .HasMaxLength(255)
                    .IsRequired();
                entity.Property(e => e.Slug)
                    .HasMaxLength(255)
                    .IsRequired();
                entity.Property(e => e.Logo)
                    .HasColumnType("nvarchar(max)")
                    .IsUnicode(false)
                    .IsRequired(false);
                entity.Property(e => e.Description)
                    .HasColumnType("nvarchar(max)")
                    .IsRequired(false);
                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .IsRequired(true);
                entity.Property(e => e.CreatedAt)
                   .HasColumnType("datetime")
                   .HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(100)
                    .IsUnicode(true);
                entity.Property(e => e.UpdatedBy)
                    .HasMaxLength(100)
                    .IsUnicode(true);
                entity.Property(e => e.Deleted)
                    .HasDefaultValue(false);
                
                // SEED
                entity.HasData(
                    new Brand { Uid = 1, BrandName = "Fresho", Slug = "fresho", Logo = "fresho.png", Status = "Active" }
                );
            });

            modelBuilder.Entity<Unit>(entity =>
            {
                entity.HasKey(e => e.Uid);
                entity.Property(e => e.UnitName)
                    .HasMaxLength(255)
                    .IsRequired();
                entity.Property(e => e.Abbreviation)
                    .HasMaxLength(50)
                    .IsRequired();
                entity.Property(e => e.Description)
                    .HasColumnType("nvarchar(max)")
                    .IsRequired(false);
                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .IsRequired(true);
                entity.Property(e => e.CreatedAt)
                   .HasColumnType("datetime")
                   .HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(100)
                    .IsUnicode(true);
                entity.Property(e => e.UpdatedBy)
                    .HasMaxLength(100)
                    .IsUnicode(true);
                entity.Property(e => e.Deleted)
                    .HasDefaultValue(false);

                // SEED
                entity.HasData(
                    new Unit { Uid = 1, UnitName = "Kilogram", Abbreviation = "kg", Status = "Active" }
                );
            });

            modelBuilder.Entity<StockStatus>(entity =>
            {
                entity.HasKey(e => e.Uid);
                entity.Property(e => e.StockName)
                    .HasMaxLength(255)
                    .IsRequired();
                entity.Property(e => e.Description)
                    .HasColumnType("nvarchar(max)")
                    .IsRequired(false);
                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .IsRequired(true);
                entity.Property(e => e.CreatedAt)
                   .HasColumnType("datetime")
                   .HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(100)
                    .IsUnicode(true);
                entity.Property(e => e.UpdatedBy)
                    .HasMaxLength(100)
                    .IsUnicode(true);
                entity.Property(e => e.Deleted)
                    .HasDefaultValue(false);

                // SEED
                entity.HasData(
                    new StockStatus { Uid = 1, StockName = "Còn hàng", Status = true }
                );
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Uid);
                entity.Property(e => e.ProductName)
                    .HasMaxLength(500)
                    .IsRequired();
                entity.HasOne(e => e.SubCategory)
                    .WithMany()
                    .HasForeignKey(e => e.SubCategoryUid)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.Description)
                    .HasColumnType("nvarchar(max)")
                    .IsRequired(false);
                entity.HasOne(e => e.Unit)
                    .WithMany()
                    .HasForeignKey(e => e.UnitUid)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.StockQuantity)
                    .HasDefaultValue(0)
                    .IsRequired();
                entity.HasOne(e => e.StockStatus)
                    .WithMany()
                    .HasForeignKey(e => e.StockStatusUid)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.Sku)
                    .HasMaxLength(50)
                    .IsRequired(false);
                entity.Property(e => e.Price)
                    .HasColumnType("decimal(18, 2)")
                    .IsRequired();
                entity.Property(e => e.Discount)
                    .HasDefaultValue(0);
                entity.Property(e => e.Thumbnail)
                    .HasColumnType("nvarchar(max)")
                    .IsUnicode(false)
                    .IsRequired(false);
                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .IsRequired();
                entity.Property(e => e.Position)
                    .HasDefaultValue(0)
                    .IsRequired();
                entity.HasOne(e => e.Brand)
                    .WithMany()
                    .HasForeignKey(e => e.BrandUid)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.Weight)
                    .HasColumnType("float");
                entity.Property(e => e.IsFeatured)
                    .HasDefaultValue(false);
                entity.Property(e => e.Exchangeable)
                    .HasDefaultValue(true);
                entity.Property(e => e.Refundable)
                    .HasDefaultValue(true);
                entity.Property(e => e.Slug)
                    .HasMaxLength(255)
                    .IsRequired(true);
                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime");
                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(100)
                    .IsUnicode(true);
                entity.Property(e => e.UpdatedBy)
                    .HasMaxLength(100)
                    .IsUnicode(true);
                entity.Property(e => e.Deleted)
                    .HasDefaultValue(false);

                // --- SEED 3 SẢN PHẨM MẪU ---
                entity.HasData(
                    new Product
                    {
                        Uid = 1,
                        ProductName = "Ớt chuông đỏ",
                        Price = 35000,
                        Thumbnail = "https://themes.pixelstrap.com/fastkart/assets/images/veg-3/cate1/1.png",
                        Slug = "ot-chuong-do",
                        Status = "Active",
                        SubCategoryUid = 1, // Khóa ngoại
                        UnitUid = 1,        // Khóa ngoại
                        StockStatusUid = 1, // Khóa ngoại
                        BrandUid = 1,       // Khóa ngoại
                        StockQuantity = 100,
                        Discount = 0,
                        Weight = 1.0,
                        IsFeatured = true,
                        Exchangeable = true,
                        Refundable = true
                    },
                    new Product
                    {
                        Uid = 2,
                        ProductName = "Bông cải xanh",
                        Price = 28000,
                        Thumbnail = "https://themes.pixelstrap.com/fastkart/assets/images/veg-3/cate1/6.png",
                        Slug = "bong-cai-xanh",
                        Status = "Active",
                        SubCategoryUid = 1, 
                        UnitUid = 1,        
                        StockStatusUid = 1, 
                        BrandUid = 1,       
                        StockQuantity = 100,
                        Discount = 0,
                        Weight = 1.0,
                        IsFeatured = false,
                        Exchangeable = true,
                        Refundable = true
                    },
                    new Product
                    {
                        Uid = 3,
                        ProductName = "Cà rốt hữu cơ",
                        Price = 22000,
                        Thumbnail = "https://themes.pixelstrap.com/fastkart/assets/images/veg-3/pro1/3.png",
                        Slug = "ca-rot-huu-co",
                        Status = "Active",
                        SubCategoryUid = 1, 
                        UnitUid = 1,        
                        StockStatusUid = 1, 
                        BrandUid = 1,       
                        StockQuantity = 100,
                        Discount = 0,
                        Weight = 1.0,
                        IsFeatured = false,
                        Exchangeable = true,
                        Refundable = true
                    }
                );
            });


            modelBuilder.Entity<Function>(entity =>
            {
                entity.HasKey(f => f.Uid);
                entity.Property(f => f.Name)
                    .IsRequired()
                    .HasMaxLength(200);
                entity.Property(f => f.Code)
                    .IsRequired()
                    .HasMaxLength(100);
                entity.Property(f => f.Status)
                    .HasMaxLength(50)
                    .HasDefaultValue("Active");
                entity.Property(f => f.Deleted)
                    .HasDefaultValue(false);
                entity.HasMany(f => f.Permissions)
                    .WithOne(p => p.Function)
                    .HasForeignKey(p => p.FunctionId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Permission>(entity =>
            {
                entity.HasKey(p => p.Uid);

                entity.HasOne(p => p.Role)
                    .WithMany(r => r.Permissions)
                    .HasForeignKey(p => p.RoleId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(p => p.Function)
                    .WithMany(f => f.Permissions)
                    .HasForeignKey(p => p.FunctionId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(p => p.PermissionType)
                    .WithMany(pt => pt.Permissions)
                    .HasForeignKey(p => p.PermissionTypeId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.Property(p => p.Allowed)
                    .HasDefaultValue(false);
            });

            modelBuilder.Entity<Roles>(entity =>
            {
                entity.HasKey(e => e.Uid);
                entity.Property(e => e.RoleName)
                    .HasMaxLength(100)
                    .IsRequired();
                entity.Property(e => e.CreatedAt)
                   .HasColumnType("datetime")
                   .HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.CreatedBy)
                   .HasMaxLength(100)
                   .IsUnicode(true)
                   .IsRequired(false);
                entity.Property(e => e.UpdatedBy)
                    .HasMaxLength(100)
                    .IsUnicode(true)
                    .IsRequired(false);
                entity.Property(e => e.Deleted)
                    .HasDefaultValue(false);
            });

            modelBuilder.Entity<PermissionType>(entity =>
            {
                entity.HasKey(pt => pt.Id);
                entity.Property(pt => pt.Name)
                    .IsRequired()
                    .HasMaxLength(100);
                entity.Property(pt => pt.Code)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.Uid);
                entity.Property(e => e.FullName)
                    .HasMaxLength(200)
                    .IsUnicode(true)
                    .IsRequired();
                entity.Property(e => e.ImgUser)
                     .HasColumnType("nvarchar(max)")
                    .IsRequired(false);

                entity.Property(e => e.Email)
                    .HasConversion(v => v.ToLower(), v => v)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .IsRequired();
                entity.HasIndex(e => e.Email)
                    .IsUnique();

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .IsRequired(false);
                entity.Property(e => e.Address)
                    .HasColumnType("nvarchar(255)")
                    .IsRequired(false);
                entity.Property(e => e.PasswordHash)
                    .HasColumnType("nvarchar(max)")
                    .IsRequired();
                entity.Property(e => e.OtpCode)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .IsRequired(false);
                entity.Property(e => e.OtpExpiry)
                    .HasColumnType("datetime")
                    .IsRequired(false);

                entity.HasOne(u => u.Role)
                    .WithMany(r => r.Users)
                    .HasForeignKey(u => u.RoleUid)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.CreatedBy)
                   .HasMaxLength(100)
                   .IsUnicode(true)
                   .IsRequired(false);
                entity.Property(e => e.UpdatedBy)
                    .HasMaxLength(100)
                    .IsUnicode(true)
                    .IsRequired(false);
                entity.Property(e => e.Deleted)
                    .HasDefaultValue(false);
            });

            modelBuilder.Entity<Cart>(entity =>
            {
                entity.HasKey(e => e.Uid);
                // Thêm các cấu hình khác cho Cart nếu cần
                // Ví dụ: liên kết với Users
                entity.HasOne(c => c.User)
                    .WithMany() // Giả sử User có thể có nhiều Cart (mặc dù thường là 1-1)
                    .HasForeignKey(c => c.UserUid)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<CartItem>(entity =>
            {
                entity.HasKey(e => e.Uid);

                // Liên kết CartItem với Cart
                entity.HasOne(ci => ci.Cart)
                    .WithMany(c => c.Items) // Giả sử Cart có 'List<CartItem> Items'
                    .HasForeignKey(ci => ci.CartUid)
                    .OnDelete(DeleteBehavior.Cascade);

                // Liên kết CartItem với Product
                entity.HasOne(ci => ci.Product)
                    .WithMany() // Product không cần biết nó ở trong CartItem nào
                    .HasForeignKey(ci => ci.ProductUid)
                    .OnDelete(DeleteBehavior.Restrict); // Không cho xóa Product nếu đang ở trong giỏ
            });

            modelBuilder.Entity<Order>(entity =>
            {
                //entity.ToTable("Orders");
                entity.HasKey(e => e.Uid);
                entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 2)");
                // Thêm các liên kết khác cho Order (ví dụ: với Users)
            });

            modelBuilder.Entity<OrderItem>(entity =>
            {
                //entity.ToTable("OrderItems");
                entity.HasKey(e => e.Uid);
                entity.Property(e => e.PriceAtPurchase).HasColumnType("decimal(18, 2)");

                // Liên kết OrderItem với Order
                entity.HasOne(oi => oi.Order)
                    .WithMany(o => o.OrderItems) // Giả sử Order có 'List<OrderItem> Items'
                    .HasForeignKey(oi => oi.OrderUid)
                    .OnDelete(DeleteBehavior.Cascade);

                // Liên kết OrderItem với Product
                entity.HasOne(oi => oi.Product)
                    .WithMany()
                    .HasForeignKey(oi => oi.ProductUid)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasKey(e => e.Uid);
                entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
                // Thêm các liên kết khác cho Payment (ví dụ: với Order)
            });

        }
    }
}