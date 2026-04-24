using Inventory_Management_System.Data;
using Inventory_Management_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Inventory_Management_System.Services
{
    public class ProductService
    {
        private readonly ApplicationDbContext _context;

        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }
        public List<Product> GetAllProducts()
        {
            return _context.Products.Include(p => p.Category).ToList();
        }

        public void AddProduct(Product product)
        {
            bool exists = _context.Products
                .Any(p => p.Name.ToLower().Trim() == product.Name.ToLower().Trim());

            if (exists)
            {
                throw new Exception("Product already exists");
            }

            if (product.CategoryId == null || product.CategoryId == 0)
            {
                throw new Exception("Please select a category");
            }

            _context.Products.Add(product);
            _context.SaveChanges();
        }
    }
}
