using Fastkart.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace Fastkart.Controllers
{
    [Route("payment")]
    public class PaymentController : Controller
    {
        private readonly MoMoService _momoService;
        private readonly IConfiguration _config;
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(MoMoService momoService, IConfiguration config, ILogger<PaymentController> logger)
        {
            _momoService = momoService;
            _config = config;
            _logger = logger;
        }

        // ======= B1: Tạo link thanh toán MoMo (ĐÃ SỬA ĐỂ NHẬN DỮ LIỆU ĐỘNG) =======
        [HttpGet("momo")]
        public async Task<IActionResult> PayWithMoMo([FromQuery] long amount, [FromQuery] string orderInfo)
        {
            // Tạo một OrderId mới, duy nhất tại đây
            string orderId = DateTime.Now.Ticks.ToString();

            // Sử dụng thông tin từ client (trang checkout gửi qua)
            // Nếu không có thì mới dùng giá trị mặc định để test
            long paymentAmount = (amount > 0) ? amount : 50000;
            string paymentInfo = !string.IsNullOrEmpty(orderInfo)
                ? orderInfo
                : "Thanh toán đơn hàng " + orderId;

            var payUrl = await _momoService.CreatePaymentAsync(paymentAmount, orderId, paymentInfo);
            _logger.LogInformation("Redirecting to MoMo PayUrl: {url}", payUrl);

            return Redirect(payUrl);
        }

        // ======= B2: Người dùng được redirect về (hiển thị kết quả) =======
        [HttpGet("return")]
        public IActionResult PaymentReturn()
        {
            var query = Request.Query;

            ViewBag.Result = query["resultCode"] == "0"
                ? "✅ Thanh toán thành công!"
                : "❌ Thanh toán thất bại hoặc bị hủy.";

            ViewBag.OrderId = query["orderId"];
            ViewBag.Amount = query["amount"];
            ViewBag.Message = query["message"];

            _logger.LogInformation("PaymentReturn Query: {query}", JsonConvert.SerializeObject(query));

            return View("Result");
        }

        // ======= B3: MoMo gọi ngược về server để xác thực chữ ký =======
        [HttpPost("notify")]
        public async Task<IActionResult> PaymentNotify()
        {
            using var reader = new StreamReader(Request.Body, Encoding.UTF8);
            var body = await reader.ReadToEndAsync();

            _logger.LogInformation("MoMo Notify Received: {body}", body);

            var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(body);
            if (data == null)
            {
                return BadRequest(new { message = "Invalid JSON" });
            }

            // Lấy key từ appsettings.json
            var secretKey = _config["MoMo:SecretKey"];
            var accessKey = _config["MoMo:AccessKey"];

            // === Dựng lại chuỗi đúng thứ tự theo tài liệu chính thức ===
            var rawHash =
                $"accessKey={accessKey}" +
                $"&amount={data["amount"]}" +
                $"&extraData={data["extraData"]}" +
                $"&message={data["message"]}" +
                $"&orderId={data["orderId"]}" +
                $"&orderInfo={data["orderInfo"]}" +
                $"&orderType={data["orderType"]}" +
                $"&partnerCode={data["partnerCode"]}" +
                $"&payType={data["payType"]}" +
                $"&requestId={data["requestId"]}" +
                $"&responseTime={data["responseTime"]}" +
                $"&resultCode={data["resultCode"]}" +
                $"&transId={data["transId"]}";

            var mySignature = CreateSignature(secretKey, rawHash);
            var momoSig = data["signature"]?.ToString();

            _logger.LogInformation("Data string to be hashed: {raw}", rawHash);
            _logger.LogInformation("My Signature: {sig}", mySignature);
            _logger.LogInformation("MoMo Signature: {sig}", momoSig);

            if (mySignature == momoSig && data["resultCode"]?.ToString() == "0")
            {
                _logger.LogInformation("✅ MoMo payment verified successfully!");
                // 👉 Cập nhật đơn hàng trong DB tại đây
                return Ok(new { message = "Payment verified successfully" });
            }
            else
            {
                _logger.LogWarning("❌ Invalid signature or payment failed!");
                return BadRequest(new { message = "Invalid signature" });
            }
        }

        // ======= Hàm hash HMAC SHA256 =======
        private static string CreateSignature(string key, string data)
        {
            var encoding = new UTF8Encoding();
            byte[] keyByte = encoding.GetBytes(key);
            byte[] messageBytes = encoding.GetBytes(data);
            using var hmacsha256 = new HMACSHA256(keyByte);
            byte[] hashMessage = hmacsha256.ComputeHash(messageBytes);
            return BitConverter.ToString(hashMessage).Replace("-", "").ToLower();
        }
    }
}
