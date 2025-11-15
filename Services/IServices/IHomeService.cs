using Fastkart.Models.Entities;
using System.Text.Json;

namespace Fastkart.Services.IServices
{
    public interface IHomeService
    {
        public List<ProductCategory> GetAllCategory();
        public List<object> GetProduct(string slug);
        public List<Product> GetAllProduct();
        public List<Product> GetNewProduct();
        public List<Product> GetFeatureProduct();
    }
}
