namespace Fastkart.Models.Entities
{
    public class OptionValue
    {
        public int Uid { get; set; }
        public int OptionNameUid { get; set; }
        public string Value { get; set; }
        public OptionName OptionName { get; set; }
        public ICollection<ProductVariantOptionValue> ProductVariantOptionValues { get; set; }
    }
}
