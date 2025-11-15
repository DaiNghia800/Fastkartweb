namespace Fastkart.Models.Entities
{
    public class Payment
    {
        public int Uid { get; set; }

        public int OrderUid { get; set; }
        public Order Order { get; set; }

        public string PaymentMethod { get; set; } // (COD, Bank Transfer QR...)
        public string PaymentStatus { get; set; } // (Pending, Completed, Failed)
        public decimal Amount { get; set; }
        public DateTime? TransactionDate { get; set; }

        // Dành cho thanh toán QR
        public string QrPaymentCode { get; set; }
        public string BankTransactionCode { get; set; } // Mã giao dịch ngân hàng trả về
    }
}