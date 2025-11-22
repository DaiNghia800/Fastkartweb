using Fastkart.Models.Entities;
using System.Text.Json;

namespace Fastkart.Services.IServices
{
    public interface ISubCategoryService
    {
        List<ProductSubCategory> GetAllSubCategory(int skip, int limitItem, string status, string keyword);
        List<ProductCategory> GetAllProductCategory();
        void CreateSubCategory(ProductSubCategory subCategory);
        int CountSubCategory(string status, string keyword);

        ProductSubCategory GetSubCategory(int id);
        void EditSubCategory(ProductSubCategory subCategory, int id);
        int DeleteSubCategory(int id);
        void ChangeStatus(int id, string status);
        string ChangeMulti(JsonElement data);
        bool checkSubCategoryName(int subId, int categoryId, string name);
    }
}
