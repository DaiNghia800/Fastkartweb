namespace Fastkart.Models.Entities
{
    public class ProductCategory
    {
        public int Uid { get; set; }
        public string CategoryName { get; set; }
        public string Thumbnail { get; set; }
        public string Icon { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string Slug { get; set; }
        public ICollection<ProductSubCategory> SubCategories { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public bool Deleted { get; set; } = false;
    }
}
