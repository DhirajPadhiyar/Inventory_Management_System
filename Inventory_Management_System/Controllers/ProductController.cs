using Inventory_Management_System.Services;
using Microsoft.AspNetCore.Mvc;
using Inventory_Management_System.Models;
using Inventory_Management_System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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
            var products = _productService.GetAllProducts();
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
            if(product.Price <= 0)
            {
                ModelState.AddModelError("Price", "Price must be greater than zero.");
            }
     
            if (product.CategoryId == null)
            {
                ModelState.AddModelError("CategoryId", "Please select a category");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _productService.AddProduct(product);
                    TempData["Success"] = $"Product '{product.Name}' added successfully!";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    // 👇 SHOW ERROR IN UI INSTEAD OF CRASH
                    ModelState.AddModelError("", ex.Message);
                }
            }

            var categories = _context.Categories.ToList();
            ViewBag.CategoryList = new SelectList(categories, "Id", "Name", product.CategoryId);

            return View(product);
        }

        //Update Action
        public IActionResult Edit(int id)
        {
            var product = _context.Products.Find(id);

            if(product==null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        public IActionResult Edit(Product model)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Duplicate check (IMPORTANT)
            if (_context.Products.Any(p => p.Name.ToLower() == model.Name.ToLower() && p.Id != model.Id))
            {
                ModelState.AddModelError("Name", "Product with this name already exists.");
                return View(model);
            }
            try
            {
               
                var product= _context.Products.Find(model.Id);
               
                if (product == null)
                {
                    return NotFound();
                }

                product.Name = model.Name;
                product.Price = model.Price;
                product.Quantity = model.Quantity;
                _context.SaveChanges(); 

                TempData["Success"] = $"Product '{model.Name}' updated successfully!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Something went wrong while updating the product.");
                return View(model);
            }
        }

        //Delete Action
        public IActionResult Delete(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                var product = _context.Products.Find(id);
                if (product == null)
                {
                    TempData["Error"] = "Product not found or already deleted.";
                    return RedirectToAction("Index");
                }

                _context.Products.Remove(product);
                _context.SaveChanges();

                TempData["Success"] = $"Product '{product.Name}' deleted successfully!";

                return RedirectToAction("Index");
                
            }
            catch (Exception ex)
            {
                TempData["error"] = $"Something went wrong while deleting the product.";
                return RedirectToAction("Index");
            }
        }
    }
}
