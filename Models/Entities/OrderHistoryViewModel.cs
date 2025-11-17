using System;

namespace Fastkart.Models.ViewModels
{
    public class OrderHistoryViewModel
    {
        public int Uid { get; set; }
        public string OrderCode { get; set; }
        public string ProductName { get; set; }
        public string Status { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime OrderDate { get; set; }

        public string StatusClass
        {
            get
            {
                return Status?.ToLower() switch
                {
                    "pending_cod" => "pending",
                    "pending_payment" => "pending",
                    "paid" => "success",
                    "shipped" => "success",
                    "cancelled" => "danger",
                    _ => "pending"
                };
            }
        }

        public string StatusLabel
        {
            get
            {
                return Status switch
                {
                    "Pending_COD" => "Processing",       
                    "Pending_Payment" => "Pending Payment", 
                    "Paid" => "Paid",                   
                    "Shipped" => "Shipped",             
                    "Cancelled" => "Cancelled",
                    _ => Status
                };
            }
        }
    }
}