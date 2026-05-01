using Inventory_Management_System.Models;
using Inventory_Management_System.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inventory_Management_System.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly CategoryService _categoryService;

        public CategoryController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // GET: Category List
        public IActionResult Index()
        {
            var categories = _categoryService.GetAll();
            return View(categories);
        }

        // GET: Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Create
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                _categoryService.Add(category);
                return RedirectToAction("Index");
            }

            return View(category);
        }

        // GET: Edit
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            var category = _categoryService.GetById(id);

            if (category == null)
                return NotFound();

            return View(category);
        }

        // POST: Edit
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                _categoryService.Update(category);
                return RedirectToAction("Index");
            }

            return View(category);
        }

        // GET: Delete
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var category = _categoryService.GetById(id);

            if (category == null)
                return NotFound();

            return View(category);
        }

        // POST: Delete
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteConfirmed(int id)
        {
            var category = _categoryService.GetById(id);

            if (category == null)
                return NotFound();

            if (_categoryService.HasProducts(id))
            {
                TempData["Error"] = "Cannot delete category. Products are using it.";
                return RedirectToAction("Index");
            }

            _categoryService.Delete(category);

            return RedirectToAction("Index");
        }
    }
}