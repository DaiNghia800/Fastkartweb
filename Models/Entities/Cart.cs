namespace Fastkart.Models.Entities
{
    public class Cart
    {
        public int Uid { get; set; }

        public int UserUid { get; set; }
        public Users User { get; set; }

        public ICollection<CartItem> Items { get; set; } = new List<CartItem>();
    }
}