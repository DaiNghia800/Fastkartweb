using CloudinaryDotNet.Actions;
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
        public DbSet<Roles> Roles { get; set; }
        public DbSet<Function> Functions { get; set; }
        public DbSet<PermissionType> PermissionTypes { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<BlogPosts> BlogPosts { get; set; }
        public DbSet<BlogCategories> BlogCategories { get; set; }

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
            modelBuilder.Entity<BlogCategories>(entity =>
            {
                entity.HasKey(e => e.Uid);
                entity.Property(e => e.Name)
                    .HasMaxLength(200)
                    .IsRequired();
            });

            modelBuilder.Entity<BlogPosts>(entity =>
            {
                entity.HasKey(e => e.Uid);

                entity.Property(e => e.Title)
                    .HasMaxLength(500)
                    .IsRequired();

                entity.Property(e => e.Content)
                    .HasColumnType("nvarchar(max)")
                    .IsRequired();

                entity.HasOne(e => e.Users)
                    .WithMany()
                    .HasForeignKey(e => e.AuthorUid)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Category)
                    .WithMany()
                    .HasForeignKey(e => e.CategoryUid)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.ImageUrl)
                    .HasMaxLength(500)
                    .IsRequired(false);

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("GETDATE()");
            });

        }
    }
}
