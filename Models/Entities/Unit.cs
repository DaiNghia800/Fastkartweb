namespace Fastkart.Models.Entities
{
    public class Unit
    {
        public int Uid { get; set; }               
        public string UnitName { get; set; }            
        public string Abbreviation { get; set; }     
        public string Description { get; set; }      
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public bool Deleted { get; set; }
    }
}
