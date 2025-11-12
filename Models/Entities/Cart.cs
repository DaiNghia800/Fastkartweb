namespace Fastkart.Models.Entities
{
    public class Cart
    {
        public int Uid { get; set; }

        public int UserUid { get; set; }
        public User User { get; set; }

        public ICollection<CartItem> Items { get; set; } = new List<CartItem>();
    }
}