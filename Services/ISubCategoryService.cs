using Fastkart.Models.Entities;
using System.Text.Json;

namespace Fastkart.Services
{
    public interface ISubCategoryService
    {
        List<ProductSubCategory> GetAllSubCategory(int skip, int limitItem, string status);
        List<ProductCategory> GetAllProductCategory();
        void CreateSubCategory(ProductSubCategory subCategory);
        int CountSubCategory(string status);

        ProductSubCategory GetSubCategory(int id);
        void EditSubCategory(ProductSubCategory subCategory, int id);
        void DeleteSubCategory(int id);
        void ChangeStatus(int id, string status);
        string ChangeMulti(JsonElement data);
    }
}
