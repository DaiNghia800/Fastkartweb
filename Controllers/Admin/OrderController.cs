using CloudinaryDotNet;
using Fastkart.Models.EF;
using Fastkart.Models.Entities;
using Fastkart.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fastkart.Controllers.Admin
{
    [Authorize(Policy = "NoCustomer")]
    [Route("/admin/orders")]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        private IQueryable<Order> GetOrdersQuery(string search)
        {
            var query = _context.Order
                .Include(o => o.User)           
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)  
                .Where(o => o.Deleted == false) 
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                search = search.Trim().ToLower();

                query = query.Where(o =>
                    o.Uid.ToString().Contains(search) ||

                    (o.User != null && o.User.FullName.ToLower().Contains(search)) ||

                    (o.User != null && o.User.Email.ToLower().Contains(search)) ||

                    (o.User != null && o.User.PhoneNumber.Contains(search)) ||

                    o.PaymentMethod.ToLower().Contains(search) ||

                    o.Status.ToLower().Contains(search)
                );
            }

            return query.OrderByDescending(o => o.OrderDate);
        }

        [HttpGet("")]
        public async Task<IActionResult> Index(string search)
        {
            ViewData["CurrentSearch"] = search;

            var query = GetOrdersQuery(search);
            var orders = await query.ToListAsync();

            //pagination
            string pageStr = Request.Query["page"];
            int page = 1;
            int limitItem = 10;
            if (!string.IsNullOrEmpty(pageStr))
            {
                int.TryParse(pageStr, out page);
            }

            int skip = (page - 1) * limitItem;
            int totalProduct = orders.Count();
            int totalPage = (int)Math.Ceiling((double)totalProduct / limitItem);
            //pagination

            var allOrders = await _context.Order
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .OrderByDescending(o => o.OrderDate)
                .Skip(skip)
                .Take(limitItem)
                .ToListAsync();

            var viewModels = allOrders.Select(o =>
            {
                string displayProductName = "Order has no items";

                if (o.OrderItems.Any())
                {
                    var firstItemName = o.OrderItems.First().Product?.ProductName ?? "Product";
                    int otherCount = o.OrderItems.Count - 1;

                    if (otherCount > 0)
                        displayProductName = $"{firstItemName} (+{otherCount} others)";
                    else
                        displayProductName = firstItemName;
                }
                return new OrderHistoryViewModel
                {
                    Uid = o.Uid,
                    OrderCode = $"#{o.Uid}",
                    ProductName = displayProductName,
                    Status = o.Status,
                    TotalPrice = o.TotalAmount,
                    OrderDate = o.OrderDate
                };
            }).ToList();
                ViewData["TotalPage"] = totalPage;
                ViewData["CurrentPage"] = page;

                return View("~/Views/Admin/Order/index.cshtml", orders);
            }

        [HttpGet("details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var order = await _context.Order
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Uid == id);

            if (order == null)
            {
                return NotFound();
            }

            return View("~/Views/Admin/Order/detail.cshtml", order);
        }

        [HttpGet("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var order = await _context.Order.FindAsync(id);
            if (order != null)
            {
                 order.Deleted = true;

                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Order deleted successfully!";
            }
            return RedirectToAction("Index");
        }

        [HttpGet("export")]
        public async Task<IActionResult> ExportOrders(string search)
        {
            var query = GetOrdersQuery(search);
            var orders = await query.ToListAsync();

            var builder = new StringBuilder();
            builder.AppendLine("Order ID,Customer,Date,Payment Method,Status,Total Amount");

            foreach (var order in orders)
            {
                string customerName = order.User != null ? order.User.FullName.Replace(",", " ") : "Guest";

                builder.AppendLine($"{order.Uid},{customerName},{order.OrderDate:yyyy-MM-dd HH:mm},{order.PaymentMethod},{order.Status},{order.TotalAmount}");
            }

            string fileName = string.IsNullOrEmpty(search) ? "orders_all.csv" : $"orders_search_{search}.csv";

            var bom = Encoding.UTF8.GetPreamble();
            var csvBytes = Encoding.UTF8.GetBytes(builder.ToString());
            var fileBytes = bom.Concat(csvBytes).ToArray();

            return File(fileBytes, "text/csv", fileName);
        }

        [HttpPost("update-status")]
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            var order = await _context.Order.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            order.Status = status;
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Order status updated successfully!";

            return RedirectToAction("Details", new { id = id });
        }
    }
}