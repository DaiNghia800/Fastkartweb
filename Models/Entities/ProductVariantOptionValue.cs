namespace Fastkart.Models.Entities
{
    public class ProductVariantOptionValue
    {
        public int Uid { get; set; }
        public int ProductVariantUid { get; set; }
        public int OptionValueUid { get; set; }
        public ProductVariant ProductVariant { get; set; }
        public OptionValue OptionValue { get; set; }
    }
}
