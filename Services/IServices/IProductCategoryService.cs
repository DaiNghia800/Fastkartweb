using Fastkart.Models.Entities;
using System.Text.Json;

namespace Fastkart.Services.IServices
{
    public interface IProductCategoryService
    {
        List<ProductCategory> GetAllProductCategory(int skip, int limitItem, string status, string keyword, string sortKey, bool descending);
        ProductCategory GetProductCategory(string slug);
        void CreateCategory(ProductCategory category);
        int CountProduct(string status, string keyword);
        ProductCategory GetProductCategory(int id);
        void EditCategory(ProductCategory productCategory, int id);
        int DeleteProduct(int id);
        void ChangeStatus(int id, string status);
        string ChangeMulti(JsonElement data);
        void ChangePosition(JsonElement data);
        bool checkCategoryName(int id, string name);
    }
}
