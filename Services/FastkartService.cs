using Fastkart.Models.EF;
using Fastkart.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fastkart.Services
{
    public class FastkartService : IFastkartService
    {
        private readonly ApplicationDbContext _context;

        public FastkartService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Product> GetAllProducts()
        {
            try
            {
                return _context.Product.Include(p => p.SubCategory).ThenInclude(p => p.ProductCategory).ToList();
            }
            catch (Exception ex) { 
                return new List<Product>();
            }
        }

        public List<ProductCategory> GetAllProductCategory()
        {
            try
            {
                return _context.ProductCategory.AsNoTracking().ToList();
            } catch(Exception ex)
            {
                return new List<ProductCategory>();
            }
        }
        public List<ProductSubCategory> GetAllProductSubCategory()
        {
            try
            {
                return _context.ProductSubCategory .AsNoTracking().ToList();
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
                return _context.ProductSubCategory.Where(t => t.CategoryUid == categoryId).AsNoTracking().ToList();
            } catch
            {
                return new List<ProductSubCategory>();
            }
        }

        public List<OptionName> GetAllOptionName()
        {
            try
            {
                return _context.OptionName.AsNoTracking().ToList();
            }
            catch (Exception ex)
            {
                return new List<OptionName>();
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

        public List<string> GetOptionValues(int optionNameId)
        {
            try
            {
                return _context.OptionValue.Where(t => t.OptionNameUid == optionNameId).Select(t => t.Value).AsNoTracking().ToList();
            }catch(Exception ex)
            {
                return new List<string>();
            }
        }
        public List<BlogCategories> GetBlogCategories()
        {
            try
            {
                return _context.BlogCategories.AsNoTracking().ToList();
            }
            catch (Exception ex)
            {
                return new List<BlogCategories>();
            }
        }
        public List<BlogPosts> GetAllBlogPosts()
        {
            try
            {
                return _context.BlogPosts.Include(b => b.Category).Include(b => b.Users).AsNoTracking().ToList();
            }
            catch (Exception ex)
            {
                return new List<BlogPosts>();
            }
        }
        
        public List<Roles> GetAllRoles()
        {
            try
            {
                return _context.Roles.AsNoTracking().ToList();
            }
            catch (Exception ex)
            {
                return new List<Roles>();
            }
        }

        public List<Users> GetAllUsers()
        {
            try
            {
                return _context.Users
                               .Include(u => u.Role)
                               .AsNoTracking()
                               .ToList();
            }
            catch (Exception ex)
            {
                return new List<Users>();
            }
        }
    }
}
