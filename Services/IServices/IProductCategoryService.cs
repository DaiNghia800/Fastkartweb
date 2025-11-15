using Fastkart.Models.Entities;
using System.Text.Json;

namespace Fastkart.Services.IServices
{
    public interface IProductCategoryService
    {
        List<ProductCategory> GetAllProductCategory(int skip, int limitItem, string status, string sortKey, bool descending);
        void CreateCategory(ProductCategory category);
        int CountProduct(string status);
        ProductCategory GetProductCategory(int id);
        void EditCategory(ProductCategory productCategory, int id);
        void DeleteProduct(int id);
        void ChangeStatus(int id, string status);
        string ChangeMulti(JsonElement data);
        void ChangePosition(JsonElement data);
    }
}
