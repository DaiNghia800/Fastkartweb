namespace Fastkart.Models.Entities
{
    public class CartItem
    {
        public int Uid { get; set; }

        public int CartUid { get; set; }
        public Cart Cart { get; set; }

        // (!!!) THAY ĐỔI QUAN TRỌNG: Liên kết trực tiếp đến Product Uid
        public int ProductUid { get; set; }
        public Product Product { get; set; }

        public int Quantity { get; set; }
    }
}