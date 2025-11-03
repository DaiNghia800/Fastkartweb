namespace Fastkart.Models.Entities
{
    public class StockStatus
    {
        public int Uid { get; set; }

        public string StockName { get; set; }

        public string Description { get; set; }

        public bool Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public bool Deleted { get; set; }
    }
}
