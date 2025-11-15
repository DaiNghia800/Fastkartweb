using System.ComponentModel.DataAnnotations;

namespace Fastkart.Models.Entities
{
    public class Product
    {
        public int Uid {  get; set; }

        [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
        [RegularExpression(@"^[\p{L}0-9\s\-]+$", ErrorMessage = "Tên sản phẩm không được chứa ký tự đặc biệt")]
        public string ProductName { get; set; }

        public int SubCategoryUid { get; set; }
        public ProductSubCategory SubCategory { get; set; }
        public string Description { get; set; }

        public int UnitUid {  get; set; }
        public Unit Unit { get; set; }
        [Required(ErrorMessage = "Số lượng tồn kho không được để trống")]
        [Range(0, double.MaxValue, ErrorMessage = "Số lượng tồn kho không được bé hơn 0")]
        public int? StockQuantity {  get; set; }
        public StockStatus StockStatus { get; set; }
        public int StockStatusUid { get; set; }

        [Required(ErrorMessage = "SKU không được để trống")]
        public string Sku {  get; set; }

        [Required(ErrorMessage = "Giá sản phẩm không được để trống")]
        [Range(0, double.MaxValue, ErrorMessage = "Giá sản phẩm phải lớn hơn 0 hoặc bằng 0")]
        public decimal? Price { get; set; }

        [Required(ErrorMessage = "Giảm giá không được để trống")]
        [Range(0, 100, ErrorMessage = "Giảm giá phải nằm trong khoảng 0–100%")]
        public int? Discount { get; set; }
        public string Thumbnail { get; set; }
        public string Status { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Vị trí sản phẩm phải lớn hơn 0")]
        public int? Position { get; set; }

        public int BrandUid { get; set; }
        public Brand Brand { get; set; }

        [Required(ErrorMessage = "Khối lượng không được để trống")]
        [Range(0, 1000, ErrorMessage = "Khối lượng không hợp lệ")]
        public Double? Weight { get; set; }
        public bool IsFeatured { get; set; }
        public bool Exchangeable { get; set; }
        public bool Refundable { get; set; }
        public string Slug { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get;set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public bool Deleted { get; set; }
    }
}
