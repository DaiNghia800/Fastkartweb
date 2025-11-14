namespace Fastkart.Models.Entities
{
    public class CartItem
    {
        public int Uid { get; set; }

        public int CartUid { get; set; }
        public Cart Cart { get; set; }

        public int ProductUid { get; set; }
        public Product Product { get; set; }

        public int Quantity { get; set; }
    }
}