using Fastkart.Models.EF;
using Fastkart.Models.Entities;
using System.Text.Json;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;

namespace Fastkart.Services
{
    public class SubCategoryService : ISubCategoryService
    {
        private readonly ApplicationDbContext _context;

        public SubCategoryService(ApplicationDbContext context)
        {
            _context = context;
        }
        public List<ProductSubCategory> GetAllSubCategory(int skip, int limitItem, string status)
        {
            try
            {
                var query = _context.ProductSubCategory
                            .Include(p => p.ProductCategory)
                            .Where(p => !p.Deleted);

                if (!string.IsNullOrEmpty(status))
                {
                    query = query.Where(p => p.Status == status);
                }

                return query
                    .OrderByDescending(p => p.Uid)
                    .Skip(skip)
                    .Take(limitItem)
                    .ToList();

            }
            catch (Exception ex)
            {
                return new List<ProductSubCategory>();
            }
        }

        public List<ProductCategory> GetAllProductCategory()
        {
            try
            {
                return _context.ProductCategory.Where(p => p.Deleted == false).AsNoTracking().ToList();
            }
            catch (Exception ex)
            {
                return new List<ProductCategory>();
            }
        }

        public void CreateSubCategory(ProductSubCategory subCategory)
        {
            try
            {
                _context.ProductSubCategory.Add(subCategory);

                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public int CountSubCategory(string status)
        {
            try
            {
                var query = _context.ProductSubCategory.AsQueryable();

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

        public ProductSubCategory GetSubCategory(int id)
        {
            try
            {
                return _context.ProductSubCategory.SingleOrDefault(p => p.Deleted == false && p.Uid == id);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public void EditSubCategory(ProductSubCategory subCategory, int id)
        {
            try
            {
                var existSubCategory = _context.ProductSubCategory.SingleOrDefault(p => p.Uid == id && !p.Deleted);

                if (existSubCategory != null)
                {
                    existSubCategory.SubCategoryName = subCategory.SubCategoryName;
                    existSubCategory.CategoryUid = subCategory.CategoryUid;
                    existSubCategory.Description = subCategory.Description;
                    existSubCategory.Status = subCategory.Status;
                    existSubCategory.Slug = subCategory.Slug;
                    existSubCategory.UpdatedAt = DateTime.Now;

                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void DeleteSubCategory(int id)
        {
            try
            {
                var subCategory = _context.ProductSubCategory.SingleOrDefault(p => p.Uid == id && !p.Deleted);
                subCategory.Deleted = true;
                subCategory.UpdatedAt = DateTime.Now;
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
                var subCategory = _context.ProductSubCategory.SingleOrDefault(p => p.Uid == id && !p.Deleted);
                if (status == "Active")
                {
                    subCategory.Status = "Inactive";
                }
                else
                {
                    subCategory.Status = "Active";
                }

                subCategory.UpdatedAt = DateTime.Now;
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
                var subCategories = _context.ProductSubCategory.Where(p => ids.Contains(p.Uid)).ToList();
                switch (status)
                {
                    case "Active":
                    case "Inactive":
                        foreach (var s in subCategories)
                        {
                            s.Status = status;
                            s.UpdatedAt = DateTime.Now;
                        }
                        _context.SaveChanges();
                        return "success";
                    case "delete":
                        foreach (var s in subCategories)
                        {
                            s.Deleted = true;
                            s.UpdatedAt = DateTime.Now;
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
    }
}
