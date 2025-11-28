using Fastkart.Models.Entities;

namespace Fastkart.Models.ViewModels
{
    public class DashboardViewModel
    {
        // 4 Ô thống kê trên cùng
        public long TotalRevenue { get; set; } // Tổng doanh thu
        public int TotalOrders { get; set; }   // Tổng số đơn hàng
        public int TotalProducts { get; set; } // Tổng số sản phẩm
        public int TotalCustomers { get; set; } // Tổng số khách hàng

        // Danh sách hiển thị
        public List<ProductCategory> Categories { get; set; } = new List<ProductCategory>();
        public List<Order> RecentOrders { get; set; } = new List<Order>();     // Bảng đơn hàng gần đây
        public List<string> ChartLabels { get; set; } = new List<string>(); // Ví dụ: ["Jan", "Feb", "Mar"...]
        public List<long> ChartData { get; set; } = new List<long>();

        public List<string> EarningLabels { get; set; } = new List<string>(); // Ngày 1, 2, 3...
        public List<long> EarningData { get; set; } = new List<long>();
    }
}