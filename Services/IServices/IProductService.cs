using Fastkart.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Text.Json;

namespace Fastkart.Services.IServices
{
    public interface IProductService
    {
        List<Product> GetAllProducts(int skip, int limitItem, string status, string sortKey, bool descending);
        List<ProductCategory> GetAllProductCategory();
        List<ProductSubCategory> GetAllProductSubCategory();
        List<Brand> GetAllBrand();
        List<Unit> GetAllUnit();
        List<ProductSubCategory> GetSubCategoryByCategory(int categoryId);
        List<StockStatus> GetAllStockStatus();
        void CreateProduct(Product product);
        Product GetProduct(int id);
        Product GetProduct(string slug);
        void EditProduct(int id, Product data);
        void DeleteProduct(int id);
        void ChangeStatus(int id, string status);
        int CountProduct(string status);
        string ChangeMulti(JsonElement data);
        void ChangePosition(JsonElement data);
        List<Product> GetProductBySubCategory(int id, int subId);
    }
}
