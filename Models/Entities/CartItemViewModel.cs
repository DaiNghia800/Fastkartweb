namespace Fastkart.Models.Entities
{
    public class CartItemViewModel
    {
        public int ProductId { get; set; }

        public string Image { get; set; }
        public string ProductName { get; set; }
        public long Price { get; set; }
        public int Quantity { get; set; }

        public long Total => Price * Quantity;
    }
}
