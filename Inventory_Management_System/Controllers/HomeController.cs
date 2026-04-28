using Inventory_Management_System.Models;
using Inventory_Management_System.Data;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Inventory_Management_System.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            try
            {
                ViewBag.TotalProducts = _context.Products.Count();
                ViewBag.TotalCategories = _context.Categories.Count();
                ViewBag.LowStock = _context.Products.Count(p => p.Quantity < 5);

                // 👉 NEW: Top 5 Low Stock Products
                var lowStockProducts = _context.Products
                    .Where(p => p.Quantity < 5)
                    .OrderBy(p => p.Quantity)
                    .Take(5)
                    .ToList();

                ViewBag.LowStockProducts = lowStockProducts;

                return View();
            }
            catch (Exception)
            {
                ViewBag.Error = "Something went wrong!";
                return View();
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
