using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fastkart.Models.Entities
{
    public class Wishlist
    {
        [Key]
        public int Uid { get; set; }

        public int UserUid { get; set; }
        [ForeignKey("UserUid")]
        public Users User { get; set; }

        public int ProductUid { get; set; }
        [ForeignKey("ProductUid")]
        public Product Product { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}