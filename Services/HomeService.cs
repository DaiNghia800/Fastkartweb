using Fastkart.Models.EF;
using Fastkart.Models.Entities;
using Fastkart.Services.IServices;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Fastkart.Services
{
    public class HomeService : IHomeService
    {
        private readonly ApplicationDbContext _context;

        public HomeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<ProductCategory> GetAllCategory()
        {
            try
            {
                return _context.ProductCategory.Where(p => p.Status == "Active" && !p.Deleted).AsNoTracking().ToList();
            }
            catch
            {
                return new List<ProductCategory>();
            }
        }

        public List<object> GetProduct(string slug)
        {
            try
            {
                var products = _context.Product
                    .Include(p => p.SubCategory)
                    .ThenInclude(sc => sc.ProductCategory)
                    .Where(p => p.SubCategory != null && p.SubCategory.ProductCategory != null && p.SubCategory.ProductCategory.Slug == slug && p.Status == "Active" && !p.Deleted)
                    .Select(p => new
                    {
                        Id = p.Uid,
                        Name = p.ProductName,
                        Thumbnail = p.Thumbnail,
                        Slug = p.Slug,
                        Price = p.Price,
                        Discount = p.Discount,
                        SubCategoryName = p.SubCategory.SubCategoryName,
                        CategorySlug = p.SubCategory.ProductCategory.Slug
                    })
                    .AsNoTracking().Take(8).Select(x => (object)x).ToList();
                return products;
       
            }catch (Exception ex)
            {
                Console.WriteLine($"LỖI CẤP THIẾT (500 LÝ TƯỞNG): {ex.Message}");
                Console.WriteLine($"STACK TRACE: {ex.StackTrace}");
                return new List<object>();
            }
        }

        public List<Product> GetAllProduct()
        {
            try
            {
                return _context.Product.Where(p => p.Status == "Active" && !p.Deleted).AsNoTracking().Take(8).ToList();
            } catch(Exception ex)
            {
                return new List<Product>();
            }
        }

        public List<Product> GetNewProduct()
        {
            try
            {
                return _context.Product.Where(p => p.Status == "Active" && !p.Deleted).OrderByDescending(p => p.Uid).Take(9).AsNoTracking().ToList();
            }
            catch (Exception ex)
            {
                return new List<Product>();
            }
        }

        public List<Product> GetFeatureProduct()
        {
            try
            {
                return _context.Product.Where(p => p.Status == "Active" && p.IsFeatured == true && !p.Deleted).Take(9).AsNoTracking().ToList();
            }
            catch (Exception ex)
            {
                return new List<Product>();
            }
        }
    }
}
