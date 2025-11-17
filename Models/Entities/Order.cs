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
        public string Status { get; set; } 

        public string PaymentMethod { get; set; }

        public bool Deleted { get; set; } = false;

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}