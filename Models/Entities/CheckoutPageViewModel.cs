using Fastkart.Models.Entities;
using System.Collections.Generic;

namespace Fastkart.Models.Entities 
{
    public class CheckoutPageViewModel
    {
        public List<CartItemViewModel> CartItems { get; set; }

        public List<string> AddressesAsStrings { get; set; }
    }
}