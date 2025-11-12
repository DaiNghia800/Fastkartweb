namespace Fastkart.Models.Entities
{
    public class Order
    {
        public int Uid { get; set; }

        public int UserUid { get; set; }
        public Users User { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public decimal TotalAmount { get; set; }
        public string ShippingAddress { get; set; }
        public string Status { get; set; } // (Pending, Processing, Shipped...)

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        // Một đơn hàng có thể có nhiều thanh toán (hoặc 1)
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}