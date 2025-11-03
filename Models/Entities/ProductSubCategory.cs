namespace Fastkart.Models.Entities
{
    public class ProductSubCategory
    {
        public int Uid { get; set; }
        public string SubCategoryName { get; set; }
        public string Slug { get; set; }
        public ProductCategory ProductCategory { get; set; }
        public int CategoryUid { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public bool Deleted { get; set; }
    }
}
