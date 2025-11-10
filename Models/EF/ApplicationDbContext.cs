using Fastkart.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fastkart.Models.EF
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }
        public DbSet<ProductCategory> ProductCategory { get; set; }
        public DbSet<ProductSubCategory> ProductSubCategory { get; set; }
        public DbSet<Brand> Brand { get; set; }
        public DbSet<Unit> Unit { get; set; }
        public DbSet<StockStatus> StockStatus { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<OptionName> OptionName { get; set; }
        public DbSet<OptionValue> OptionValue { get; set; }
        public DbSet<ProductVariant> ProductVariant { get; set; }
        public DbSet<ProductVariantOptionValue> ProductVariantOptionValue { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<Users> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProductCategory>(entity =>
            {
                entity.HasKey(e => e.Uid);
                entity.Property(e => e.CategoryName)
                    .HasMaxLength(255)
                    .IsRequired();
                entity.Property(e => e.Thumbnail)
                    .HasMaxLength(255)
                    .IsRequired(false);
                entity.Property(e => e.Icon)
                    .HasMaxLength(255)
                    .IsRequired(false);
                entity.Property(e => e.Description)
                    .HasColumnType("nvarchar(max)")
                    .IsRequired(false);
                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .IsRequired(true);
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
                    .IsRequired();
                entity.Property(e => e.Price)
                    .HasColumnType("decimal(18, 2)")
                    .IsRequired();
                entity.Property(e => e.Discount)
                    .HasDefaultValue(0);
                entity.Property(e => e.Thumbnail)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .IsRequired(false);
                entity.Property(e => e.Status)
                    .HasMaxLength(50)
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
            });

            modelBuilder.Entity<OptionName>(entity =>
            {
                entity.HasKey(e => e.Uid);

                entity.Property(e => e.Name)
                      .IsRequired()
                      .HasMaxLength(100);
                entity.Property(e => e.ValueType)
                      .HasMaxLength(50)
                      .HasDefaultValue("text");

                entity.Property(e => e.Pattern)
                      .HasMaxLength(255)
                      .IsRequired(false);
                entity.HasMany(e => e.OptionValues)
                      .WithOne(v => v.OptionName)
                      .HasForeignKey(v => v.OptionNameUid)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<OptionValue>(entity =>
            {
                entity.HasKey(e => e.Uid);

                entity.Property(e => e.Value)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.HasIndex(e => new { e.OptionNameUid, e.Value })
                      .IsUnique();

                entity.HasOne(e => e.OptionName)
                      .WithMany(n => n.OptionValues)
                      .HasForeignKey(e => e.OptionNameUid)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ProductVariant>(entity =>
            {
                entity.HasKey(e => e.Uid);
                entity.HasOne(e => e.Product)
                    .WithMany(p => p.Variants)
                    .HasForeignKey(e => e.ProductUid)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.Property(e => e.VariantName)
                    .HasMaxLength(255)
                    .IsRequired();
                entity.Property(e => e.Price)
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();
                entity.Property(e => e.Sku)
                    .HasMaxLength(100)
                    .IsRequired();
                entity.Property(e => e.Quantity)
                    .HasDefaultValue(0);
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
            });

            modelBuilder.Entity<ProductVariantOptionValue>(entity =>
            {
                entity.HasKey(e => e.Uid);

                entity.HasOne(e => e.ProductVariant)
                      .WithMany(v => v.ProductVariantOptionValues)
                      .HasForeignKey(e => e.ProductVariantUid)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.OptionValue)
                      .WithMany(v => v.ProductVariantOptionValues)
                      .HasForeignKey(e => e.OptionValueUid)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(e => new { e.ProductVariantUid, e.OptionValueUid })
                      .IsUnique();
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
            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.Uid);
                entity.Property(e => e.FullName)
                    .HasMaxLength(200)
                    .IsUnicode(true)
                    .IsRequired();
                entity.Property(e => e.ImgUser)
                    .HasMaxLength(255)
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
        }
    }
}
