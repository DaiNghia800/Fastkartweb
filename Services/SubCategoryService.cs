using Fastkart.Models.EF;
using Fastkart.Models.Entities;
using System.Text.Json;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using Fastkart.Services.IServices;

namespace Fastkart.Services
{
    public class SubCategoryService : ISubCategoryService
    {
        private readonly ApplicationDbContext _context;

        public SubCategoryService(ApplicationDbContext context)
        {
            _context = context;
        }
        public List<ProductSubCategory> GetAllSubCategory(int skip, int limitItem, string status, string keyword)
        {
            try
            {
                var query = _context.ProductSubCategory
                            .Include(p => p.ProductCategory)
                            .Where(p => !p.Deleted);

                if (!string.IsNullOrWhiteSpace(keyword))
                    query = query.Where(p => p.SubCategoryName.Contains(keyword));

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
                subCategory.SubCategoryName = subCategory.SubCategoryName.Trim();
                _context.ProductSubCategory.Add(subCategory);

                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public int CountSubCategory(string status, string keyword)
        {
            try
            {
                var query = _context.ProductSubCategory.AsQueryable();

                if (!string.IsNullOrWhiteSpace(keyword))
                    query = query.Where(p => p.SubCategoryName.Contains(keyword));

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
                    existSubCategory.SubCategoryName = subCategory.SubCategoryName.Trim();
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

        public int DeleteSubCategory(int id)
        {
            try
            {
                var product = _context.Product.Count(p => p.SubCategoryUid == id && !p.Deleted);
                if(product > 0)
                {
                    return product;
                } else
                {
                    var subCategory = _context.ProductSubCategory.SingleOrDefault(p => p.Uid == id && !p.Deleted);
                    subCategory.Deleted = true;
                    subCategory.UpdatedAt = DateTime.Now;
                    _context.SaveChanges();

                    return 0;
                }
                    
            }
            catch (Exception ex)
            {
                return -1;
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

        public bool checkSubCategoryName(int subId, int categoryId, string name)
        {
            try
            {
                var normalizedName = name.Trim().ToLower();
                if (subId > 0)
                {
                    return _context.ProductSubCategory.Any(p => p.CategoryUid == categoryId && p.Uid != subId && p.SubCategoryName.ToLower() == normalizedName && !p.Deleted);
                } else
                {
                    return _context.ProductSubCategory.Any(p => p.CategoryUid == categoryId && p.SubCategoryName.ToLower() == normalizedName && !p.Deleted);
                }
            } catch(Exception ex)
            {
                return false;
            }
        }
    }
}
