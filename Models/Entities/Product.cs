namespace Fastkart.Models.Entities
{
    public class Product
    {
        public int Uid {  get; set; }
        public string ProductName { get; set; }
        public int SubCategoryUid { get; set; }
        public ProductSubCategory SubCategory { get; set; }
        public string Description { get; set; }
        public int UnitUid {  get; set; }
        public Unit Unit { get; set; }
        public int StockQuantity {  get; set; }
        public StockStatus StockStatus { get; set; }
        public int StockStatusUid { get; set; }
        public string Sku {  get; set; }
        public decimal Price { get; set; }
        public int  Discount { get; set; }
        public string Thumbnail { get; set; }
        public string Status { get; set; }
        public int BrandUid { get; set; }
        public Brand Brand { get; set; }
        public Double? Weight { get; set; }
        public bool IsFeatured { get; set; }
        public bool Exchangeable { get; set; }
        public bool Refundable { get; set; }
        public string Slug { get; set; }
        public ICollection<ProductVariant> Variants { get; set; } = new List<ProductVariant>();
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get;set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public bool Deleted { get; set; }
    }
}
