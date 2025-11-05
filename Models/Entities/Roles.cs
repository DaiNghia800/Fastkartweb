namespace Fastkart.Models.Entities
{
    public class Roles
    {
        public int Uid { get; set; }
        public string RoleName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public bool Deleted { get; set; }
        public ICollection<Users> Users { get; set; } = new List<Users>();
    }
}
