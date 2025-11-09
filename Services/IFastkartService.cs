using Fastkart.Models.Entities;

namespace Fastkart.Services
{
    public interface IFastkartService
    {
        List<Product> GetAllProducts();
        List<ProductCategory> GetAllProductCategory();
        List<ProductSubCategory> GetAllProductSubCategory();
        List<Brand> GetAllBrand();
        List<Unit> GetAllUnit();
        List<ProductSubCategory> GetSubCategoryByCategory(int categoryId);
        List<OptionName> GetAllOptionName();
        List<StockStatus> GetAllStockStatus();
        List<string> GetOptionValues(int optionNameId);
        List<BlogCategories> GetBlogCategories();
        List<BlogPosts> GetAllBlogPosts();
        List<Roles> GetAllRoles();
        List<Users> GetAllUsers();
    }
}
