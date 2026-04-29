using Inventory_Management_System.Services;
using Microsoft.AspNetCore.Mvc;
using Inventory_Management_System.Models;
using Inventory_Management_System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Inventory_Management_System.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductService _productService;
        private readonly ApplicationDbContext _context;

        public ProductController(ProductService productService, ApplicationDbContext context)
        {
            _productService = productService;
            _context = context;
        }

        public IActionResult Index()
        {
            var products = _productService.GetAllProductsForView();
            return View(products);
        }

        public IActionResult Create()
        {
            var categories = _context.Categories.ToList();
            ViewBag.CategoryList = new SelectList(categories, "Id", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult Create(Product product)
        {
            if (!ModelState.IsValid)
            {
                var categories = _context.Categories.ToList();
                ViewBag.CategoryList = new SelectList(categories, "Id", "Name", product.CategoryId);
                return View(product);
            }

            try
            {
                _productService.AddProduct(product);
                TempData["Success"] = $"Product '{product.Name}' added successfully!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            var cats = _context.Categories.ToList();
            ViewBag.CategoryList = new SelectList(cats, "Id", "Name", product.CategoryId);

            return View(product);
        }

        // GET Edit
        public IActionResult Edit(int id)
        {
            var product = _productService.GetById(id);

            if (product == null)
                return NotFound();

            var categories = _context.Categories.ToList();
            ViewBag.CategoryList = new SelectList(categories, "Id", "Name", product.CategoryId);

            return View(product);
        }

        // POST Edit
        [HttpPost]
        public IActionResult Edit(Product model)
        {
            if (!ModelState.IsValid)
            {
                var categories = _context.Categories.ToList();
                ViewBag.CategoryList = new SelectList(categories, "Id", "Name", model.CategoryId);
                return View(model);
            }

            try
            {
                _productService.UpdateProduct(model);
                TempData["Success"] = $"Product '{model.Name}' updated successfully!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            var cats = _context.Categories.ToList();
            ViewBag.CategoryList = new SelectList(cats, "Id", "Name", model.CategoryId);

            return View(model);
        }

        // GET Delete
        public IActionResult Delete(int id)
        {
            var product = _productService.GetById(id);

            if (product == null)
                return NotFound();

            return View(product);
        }

        // POST Delete
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                _productService.DeleteProduct(id);
                TempData["Success"] = "Product deleted successfully!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction("Index");
        }
        public IActionResult StockHistory(int productId)
        {
            var history = _context.stockHistories
                .Include(x=>x.product)
                .Where(x => x.ProductId == productId)
                .OrderByDescending(x => x.CreatedAt)
                .ToList();

            return View(history);
        }
    }
}