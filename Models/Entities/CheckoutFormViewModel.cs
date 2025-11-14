namespace Fastkart.Models.Entities // Hoặc namespace của ViewModel
{
    public class CheckoutFormViewModel
    {
        public string AddressId { get; set; }
        public string DeliveryOption { get; set; }
        public string PaymentMethod { get; set; }

        // Bạn có thể thêm các trường khác từ form nếu muốn
        // public string Note { get; set; }
    }
}