using System.ComponentModel.DataAnnotations;

namespace Fastkart.Models.Entities
{
    public class ProductCategory
    {
        public int Uid { get; set; }

        [Required(ErrorMessage = "Danh mục sản phẩm không được để trống")]
        [RegularExpression(@"^[a-zA-ZÀ-ỹ\s']+$", ErrorMessage = "Danh mục sản phẩm không được chứa ký tự đặc biệt")]
        public string CategoryName { get; set; }
        public string Thumbnail { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Vị trí danh mục sản phẩm phải lớn hơn 0")]
        public int? Position { get; set; }
        public string Slug { get; set; }
        public ICollection<ProductSubCategory> SubCategories { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public bool Deleted { get; set; } = false;
    }
}
