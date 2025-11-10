using System.ComponentModel.DataAnnotations;

namespace Fastkart.Models.Entities
{
    public class ProductSubCategory
    {
        public int Uid { get; set; }

        [Required(ErrorMessage = "Danh mục không được để trống")]
        [RegularExpression(@"^[\p{L}0-9\s\-]+$", ErrorMessage = "Danh mục không được chứa ký tự đặc biệt")]
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
