using Inventory_Management_System.Models;
using Inventory_Management_System.Data;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;

namespace Inventory_Management_System.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {

        private readonly ApplicationDbContext _context;
        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }
        [Authorize]
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

        public IActionResult Error()
        {
            return View();
        }
    }
}
