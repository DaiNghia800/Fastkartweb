using Fastkart.Models.EF;
using Fastkart.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fastkart.Controllers.Admin
{
    [Route("/admin/dashboard")]
    [Authorize(Policy = "NoCustomer")]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("")]
        public async Task<IActionResult> Dashboard()
        {
            var model = new DashboardViewModel();

            // 1. Tính Tổng Doanh Thu (Chỉ tính các đơn đã thanh toán hoặc hoàn thành)
            // Giả sử trạng thái "Paid" hoặc "Completed" là tính doanh thu
            model.TotalRevenue = (long)await _context.Order
                .Where(o => o.Status == "Paid" || o.Status == "Completed" || o.Status == "Delivered")
                .SumAsync(o => o.TotalAmount);

            // 2. Tổng số đơn hàng
            model.TotalOrders = await _context.Order.CountAsync();

            // 3. Tổng số sản phẩm
            model.TotalProducts = await _context.Product.CountAsync();

            // 4. Tổng số khách hàng (User có Role là Customer)
            // Lưu ý: Cần điều chỉnh string "Customer" cho đúng tên Role trong DB của bạn
            model.TotalCustomers = await _context.Users
                .Where(u => u.Role.RoleName == "Customer")
                .CountAsync();

            // 5. Lấy danh sách danh mục (Lấy 10 cái đầu tiên để hiển thị slide)
            model.Categories = await _context.ProductCategory.Take(10).ToListAsync();

            // 6. Lấy 5 đơn hàng mới nhất (để hiển thị bảng Recent Orders)
            model.RecentOrders = await _context.Order
                .Include(o => o.User) // Include User để lấy tên khách nếu cần
                .OrderByDescending(o => o.OrderDate) // Sắp xếp giảm dần theo ngày
                .Take(5)
                .ToListAsync();

            // 7. TÍNH TOÁN BIỂU ĐỒ (6 tháng gần nhất)
            var revenueData = new List<long>();
            var labels = new List<string>();

            // Lặp từ 5 tháng trước đến tháng hiện tại
            for (int i = 5; i >= 0; i--)
            {
                var date = DateTime.Now.AddMonths(-i);
                var month = date.Month;
                var year = date.Year;

                // Tính tổng tiền đơn hàng PAID trong tháng đó
                var sum = await _context.Order
                    .Where(o => o.OrderDate.Month == month && o.OrderDate.Year == year
                                && (o.Status == "Paid" || o.Status == "Completed"))
                    .SumAsync(o => (long)o.TotalAmount);

                revenueData.Add(sum);
                labels.Add(date.ToString("MMM", System.Globalization.CultureInfo.InvariantCulture)); // Tên tháng tiếng Anh (Jan, Feb...)
            }

            model.ChartData = revenueData;
            model.ChartLabels = labels;

            // 8. TÍNH TOÁN BIỂU ĐỒ EARNING (Doanh thu theo ngày trong tháng này)
            var earningData = new List<long>();
            var earningLabels = new List<string>();

            var today = DateTime.Now;
            var startOfMonth = new DateTime(today.Year, today.Month, 1);
            var daysInMonth = DateTime.DaysInMonth(today.Year, today.Month);

            // Lấy tất cả đơn hàng trong tháng này trước (để đỡ query DB nhiều lần trong vòng lặp)
            var ordersInMonth = await _context.Order
                .Where(o => o.OrderDate >= startOfMonth
                            && o.OrderDate <= today
                            && (o.Status == "Paid" || o.Status == "Completed"))
                .ToListAsync();

            // Chạy vòng lặp từ ngày 1 đến ngày hiện tại (hoặc hết tháng)
            for (int i = 1; i <= daysInMonth; i++)
            {
                // Tính tổng tiền của ngày thứ i
                var dailySum = ordersInMonth
                    .Where(o => o.OrderDate.Day == i)
                    .Sum(o => (long)o.TotalAmount);

                earningData.Add(dailySum);
                earningLabels.Add(i.ToString()); // Nhãn là ngày (1, 2, 3...)
            }

            model.EarningData = earningData;
            model.EarningLabels = earningLabels;

            return View("~/Views/Admin/Dashboard.cshtml", model);
        }
    }
}
