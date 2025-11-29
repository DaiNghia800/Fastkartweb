using System.Text;
using Fastkart.Models.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace Fastkart.Services
{
    // Class cấu hình giữ nguyên
    public class EmailSettings
    {
        public string SmtpServer { get; set; }
        public int SmtpPort { get; set; }
        public string SenderEmail { get; set; }
        public string SenderPassword { get; set; }
    }

    public class EmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IConfiguration config)
        {
            _emailSettings = config.GetSection("EmailSettings").Get<EmailSettings>();
        }

        public async Task SendEmailAsync(string toEmail, string subject, string htmlMessage)
        {
            try
            {
                var client = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.SmtpPort)
                {
                    Credentials = new NetworkCredential(_emailSettings.SenderEmail, _emailSettings.SenderPassword),
                    EnableSsl = true
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_emailSettings.SenderEmail, "Fastkart"),
                    Subject = subject,
                    Body = htmlMessage,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(toEmail);
                await client.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fail send email: " + ex.Message);
            }
        }

        // --- CẬP NHẬT HÀM NÀY ĐỂ THÊM SHIP & DISCOUNT ---
        public async Task SendOrderConfirmationAsync(Order order)
        {
            if (string.IsNullOrEmpty(order.User?.Email)) return;

            try
            {
                string subject = $"[Fastkart] Order Confirmation #{order.Uid}";

                // 1. Tính toán lại Subtotal từ danh sách sản phẩm
                long calculatedSubtotal = 0;
                if (order.OrderItems != null)
                {
                    calculatedSubtotal = (long)order.OrderItems.Sum(i => i.PriceAtPurchase * i.Quantity);
                }

                // 2. Khai báo phí ship và giảm giá (Khớp với Controller của bạn)
                // *Lưu ý: Sau này nếu lưu vào DB thì lấy từ order.ShippingFee
                long shippingFee = 25000;
                long discount = 10000;

                var sb = new StringBuilder();
                sb.Append($"<h1>Thank you for your order!</h1>");
                sb.Append($"<p>Hello <strong>{order.User?.FullName ?? "Customer"}</strong>,</p>");
                sb.Append($"<p>Your order <strong>#{order.Uid}</strong> has been confirmed.</p>");
                sb.Append($"<p>Payment Method: <strong>{order.PaymentMethod}</strong></p>");

                // Bắt đầu bảng
                sb.Append("<table border='1' cellpadding='10' cellspacing='0' style='border-collapse: collapse; width: 100%; font-family: Arial, sans-serif;'>");
                sb.Append("<tr style='background-color: #f2f2f2;'><th>Product</th><th>Qty</th><th>Price</th><th>Total</th></tr>");

                if (order.OrderItems != null)
                {
                    foreach (var item in order.OrderItems)
                    {
                        var productName = item.Product?.ProductName ?? "Unknown Item";
                        var totalItemPrice = item.PriceAtPurchase * item.Quantity;
                        sb.Append("<tr>");
                        sb.Append($"<td>{productName}</td>");
                        sb.Append($"<td style='text-align:center'>{item.Quantity}</td>");
                        sb.Append($"<td style='text-align:right'>{item.PriceAtPurchase:N0} VND</td>");
                        sb.Append($"<td style='text-align:right'>{totalItemPrice:N0} VND</td>");
                        sb.Append("</tr>");
                    }
                }

                // --- PHẦN TỔNG CỘNG (MỚI) ---

                // Dòng Subtotal (Tạm tính)
                sb.Append($"<tr><td colspan='3' style='text-align:right'>Subtotal:</td><td style='text-align:right'>{calculatedSubtotal:N0} VND</td></tr>");

                // Dòng Shipping
                sb.Append($"<tr><td colspan='3' style='text-align:right'>Shipping Fee:</td><td style='text-align:right'>{shippingFee:N0} VND</td></tr>");

                // Dòng Discount
                sb.Append($"<tr><td colspan='3' style='text-align:right'>Discount:</td><td style='text-align:right'>-{discount:N0} VND</td></tr>");

                // Dòng Grand Total (Tổng thanh toán)
                sb.Append($"<tr style='background-color: #e6e6e6;'><td colspan='3' style='text-align:right'><strong>Grand Total:</strong></td><td style='text-align:right; color: #0da487;'><strong>{order.TotalAmount:N0} VND</strong></td></tr>");

                sb.Append("</table>");

                sb.Append("<p>We will ship your items as soon as possible.</p>");
                sb.Append("<p>Best Regards,<br/>Fastkart Team</p>");

                string body = sb.ToString();

                await SendEmailAsync(order.User.Email, subject, body);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi gửi mail order: {ex.Message}");
            }
        }
    }
}