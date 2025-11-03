namespace Fastkart.Models.Entities
{
    public class OptionName
    {
        public int Uid { get; set; }
        public string Name { get; set; }
        public string ValueType { get; set; }
        public string Pattern { get; set; }
        public ICollection<OptionValue> OptionValues { get; set; }
    }
}
