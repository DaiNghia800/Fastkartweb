namespace Fastkart.Models.Entities
{
    public class OrderItem
    {
        public int Uid { get; set; }

        public int OrderUid { get; set; }
        public Order Order { get; set; }

        // (!!!) THAY ĐỔI QUAN TRỌNG: Liên kết trực tiếp đến Product Uid
        public int ProductUid { get; set; }
        public Product Product { get; set; }

        public int Quantity { get; set; }
        public decimal PriceAtPurchase { get; set; } // Lưu lại giá tại thời điểm mua
    }
}