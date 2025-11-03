namespace Fastkart.Models.Entities
{
    public class ProductVariant
    {
        public int Uid { get; set; }
        public int ProductUid { get; set; }
        public Product Product { get; set; }
        public string VariantName { get; set; }
        public decimal Price { get; set; }
        public string Sku { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public bool Deleted { get; set; }
        public ICollection<ProductVariantOptionValue> ProductVariantOptionValues { get; set; }
    }
}