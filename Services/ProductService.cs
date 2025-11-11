using Fastkart.Models.EF;
using Fastkart.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Text.Json;

namespace Fastkart.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;

        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Product> GetAllProducts(int skip, int limitItem, string status, string sortKey, bool descending)
        {
            try
            {
                var query = _context.Product
                            .Include(p => p.SubCategory)
                            .ThenInclude(p => p.ProductCategory)
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
            catch (Exception ex) { 
                return new List<Product>();
            }
        }

        public List<ProductCategory> GetAllProductCategory()
        {
            try
            {
                return _context.ProductCategory.Where(p => p.Deleted == false && p.Status == "Active").AsNoTracking().ToList();
            } catch(Exception ex)
            {
                return new List<ProductCategory>();
            }
        }
        public List<ProductSubCategory> GetAllProductSubCategory()
        {
            try
            {
                return _context.ProductSubCategory.Where(p => p.Status == "Active" && !p.Deleted).AsNoTracking().ToList();
            } catch(Exception ex)
            {
                return new List<ProductSubCategory>();
            }
        }
        public List<Brand> GetAllBrand()
        {
            try
            {
                return _context.Brand.AsNoTracking().ToList();
            } catch(Exception ex)
            {
                return new List<Brand>();
            }
        }
        public List<Unit> GetAllUnit()
        {
            try
            {
                return _context.Unit.AsNoTracking().ToList();
            } catch(Exception ex)
            {
                return new List<Unit>();
            }
        }

        public List<ProductSubCategory> GetSubCategoryByCategory(int categoryId)
        {
            try
            {
                return _context.ProductSubCategory.Where(t => t.CategoryUid == categoryId && t.Status == "Active" && !t.Deleted).AsNoTracking().ToList();
            } catch
            {
                return new List<ProductSubCategory>();
            }
        }

        public List<StockStatus> GetAllStockStatus()
        {
            try
            {
                return _context.StockStatus.AsNoTracking().ToList();
            }
            catch (Exception ex)
            {
                return new List<StockStatus>();
            }
        }

        public void CreateProduct(Product product)
        {
            _context.Product.Add(product);

            _context.SaveChanges();
        }

        public Product GetProduct(int id)
        {
            try
            {
                return _context.Product.Include(p => p.SubCategory).SingleOrDefault(p => p.Uid == id && !p.Deleted); 
            } catch (Exception ex)
            {
                return null;
            }
        }

        public void EditProduct(int id, Product data)
        {
            try
            {
                var product = _context.Product.SingleOrDefault(p => p.Uid == id && !p.Deleted);

                if (product != null)
                {
                    product.ProductName = data.ProductName;
                    product.SubCategoryUid = data.SubCategoryUid;
                    product.Description = data.Description;
                    product.UnitUid = data.UnitUid;
                    product.StockQuantity = data.StockQuantity;
                    product.StockStatusUid = data.StockStatusUid;
                    product.Sku = data.Sku;
                    product.Price = data.Price;
                    product.Discount = data.Discount;
                    product.Thumbnail = data.Thumbnail;
                    product.Status = data.Status;
                    product.Position = data.Position;
                    product.BrandUid = data.BrandUid;
                    product.Weight = data.Weight;
                    product.IsFeatured = data.IsFeatured;
                    product.Exchangeable = data.Exchangeable;
                    product.Slug = data.Slug;
                    product.UpdatedAt = DateTime.Now;

                    _context.SaveChanges();
                }
            } catch (Exception ex)
            {
                throw;
            }
        }

        public void DeleteProduct(int id)
        {
            try
            {
                var product = _context.Product.SingleOrDefault(p => p.Uid == id && !p.Deleted);
                product.Deleted = true;
                product.UpdatedAt = DateTime.Now;
                _context.SaveChanges();
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public void ChangeStatus(int id, string status)
        {
            try
            {
                var product = _context.Product.SingleOrDefault(p => p.Uid == id && !p.Deleted);
                if(status == "Active")
                {
                    product.Status = "Inactive";
                } else
                {
                    product.Status = "Active";
                }

                product.UpdatedAt = DateTime.Now;
                _context.SaveChanges();
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public int CountProduct(string status)
        {
            try
            {
                var query = _context.Product.AsQueryable();

                if (!string.IsNullOrEmpty(status))
                {
                    query = query.Where(p => p.Status == status);
                }

                query = query.Where(p => !p.Deleted);
                return query.Count();
            }catch(Exception ex)
            {
                return 0;
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
                var products = _context.Product.Where(p => ids.Contains(p.Uid)).ToList();
                switch (status)
                {
                    case "Active":
                    case "Inactive":
                        foreach(var p in products)
                        {
                            p.Status = status;
                            p.UpdatedAt = DateTime.Now;
                        }
                        _context.SaveChanges();
                        return "success";
                    case "delete":
                        foreach (var p in products)
                        {
                            p.Deleted = true;
                            p.UpdatedAt = DateTime.Now;
                        }

                        _context.SaveChanges();
                        return "deleted";
                    default:
                        return "invalid";
                }
                
                
            }catch(Exception ex)
            {
                return null;
            }
        }

        public void ChangePosition(JsonElement data)
        {
            int id = data.GetProperty("id").GetInt32();
            int position = data.GetProperty("position").GetInt32();

            var product = _context.Product.SingleOrDefault(p => p.Uid == id && !p.Deleted);
            if(product != null)
            {
                product.Position = position;
                product.UpdatedAt = DateTime.Now;
            }
            _context.SaveChanges();
        }
    }
}
