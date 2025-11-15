using Fastkart.Models.EF;
using Fastkart.Models.Entities;
using Fastkart.Services.IServices;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Linq.Dynamic.Core;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Fastkart.Services
{
    public class ProductCategoryService : IProductCategoryService
    {
        private readonly ApplicationDbContext _context;

        public ProductCategoryService(ApplicationDbContext context)
        {
            _context = context;
        }
        public List<ProductCategory> GetAllProductCategory(int skip, int limitItem, string status, string sortKey, bool descending)
        {
            try
            {
                var query = _context.ProductCategory
                            .Where(p => !p.Deleted);

                if (!string.IsNullOrEmpty(status))
                {
                    query = query.Where(p => p.Status == status);
                }

                query = query.OrderBy($"{sortKey} {(descending ? "descending" : "ascending")}");

                return query
                    .Skip(skip)
                    .Take(limitItem)
                    .ToList();

            }
            catch (Exception ex)
            {
                return new List<ProductCategory>();
            }
        }

        
        public void CreateCategory(ProductCategory category) {
            try
            {
                _context.ProductCategory.Add(category);

                _context.SaveChanges();
            }
            catch (Exception ex) {
                throw;
            }
        }

        public int CountProduct(string status)
        {
            try
            {
                var query = _context.ProductCategory.AsQueryable();

                if (!string.IsNullOrEmpty(status))
                {
                    query = query.Where(p => p.Status == status);
                }

                query = query.Where(p => !p.Deleted);
                return query.Count();
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public ProductCategory GetProductCategory(int id)
        {
            try
            {
                return _context.ProductCategory.SingleOrDefault(p => p.Deleted == false && p.Uid == id);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public void EditCategory(ProductCategory productCategory, int id)
        {
            try
            {
                var existCategory = _context.ProductCategory.SingleOrDefault(p => p.Uid == id && !p.Deleted);

                if (existCategory != null)
                {
                    existCategory.CategoryName = productCategory.CategoryName;
                    existCategory.Description = productCategory.Description;
                    existCategory.Thumbnail = productCategory.Thumbnail;
                    existCategory.Status = productCategory.Status;
                    existCategory.Position = productCategory.Position;
                    existCategory.Slug = productCategory.Slug;
                    existCategory.UpdatedAt = DateTime.Now;

                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void DeleteProduct(int id)
        {
            try
            {
                var product = _context.ProductCategory.SingleOrDefault(p => p.Uid == id && !p.Deleted);
                product.Deleted = true;
                product.UpdatedAt = DateTime.Now;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void ChangeStatus(int id, string status)
        {
            try
            {
                var product = _context.ProductCategory.SingleOrDefault(p => p.Uid == id && !p.Deleted);
                if (status == "Active")
                {
                    product.Status = "Inactive";
                }
                else
                {
                    product.Status = "Active";
                }

                product.UpdatedAt = DateTime.Now;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public string ChangeMulti(JsonElement data)
        {
            try
            {
                var ids = data.GetProperty("id").EnumerateArray()
                    .Select(x =>
                    {
                        if (x.ValueKind == JsonValueKind.String)
                            return int.Parse(x.GetString());
                        else
                            return x.GetInt32();
                    })
                    .ToList();
                string status = data.GetProperty("status").GetString();
                var categories = _context.ProductCategory.Where(p => ids.Contains(p.Uid)).ToList();
                switch (status)
                {
                    case "Active":
                    case "Inactive":
                        foreach (var c in categories)
                        {
                            c.Status = status;
                            c.UpdatedAt = DateTime.Now;
                        }
                        _context.SaveChanges();
                        return "success";
                    case "delete":
                        foreach (var c in categories)
                        {
                            c.Deleted = true;
                            c.UpdatedAt = DateTime.Now;
                        }

                        _context.SaveChanges();
                        return "deleted";
                    default:
                        return "invalid";
                }


            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public void ChangePosition(JsonElement data)
        {
            int id = data.GetProperty("id").GetInt32();
            int position = data.GetProperty("position").GetInt32();

            var category = _context.ProductCategory.SingleOrDefault(p => p.Uid == id && !p.Deleted);
            if (category != null)
            {
                category.Position = position;
                category.UpdatedAt = DateTime.Now;
            }
            _context.SaveChanges();
        }
    }
}
