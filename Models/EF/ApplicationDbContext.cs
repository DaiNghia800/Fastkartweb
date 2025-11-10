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

           
        }
    }
}
