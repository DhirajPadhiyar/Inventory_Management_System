using Inventory_Management_System.Data;
using Inventory_Management_System.DTOs;
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



        public List<ProductDTO> GetAllProducts()
        {
            return _context.Products
                .Select(p => new ProductDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    CategoryName = p.Category.Name
                })
                .ToList();
        }

        public List<Product> GetAllProductsForView()
        {
            return _context.Products
                .Include(p => p.Category)
                .ToList();
        }

        public Product GetById(int id)
        {
            return _context.Products.Find(id);
        }

        public void AddProduct(Product product)
        {
            if (product.Price <= 0)
                throw new Exception("Price must be greater than zero.");

            if (product.CategoryId == null || product.CategoryId == 0)
                throw new Exception("Please select a category.");

            bool exists = _context.Products
                .Any(p => p.Name.ToLower().Trim() == product.Name.ToLower().Trim());

            if (exists)
                throw new Exception("Product already exists.");

            _context.Products.Add(product);
            _context.SaveChanges();
        }

        public void UpdateProduct(Product model)
        {
            if (model.Price <= 0)
                throw new Exception("Price must be greater than zero.");

            bool exists = _context.Products
                .Any(p => p.Name.ToLower().Trim() == model.Name.ToLower().Trim() && p.Id != model.Id);

            if (exists)
                throw new Exception("Product with this name already exists.");

            var product = _context.Products.Find(model.Id);

            if (product == null)
                throw new Exception("Product not found.");

            product.Name = model.Name;
            product.Price = model.Price;
            product.Quantity = model.Quantity;
            product.CategoryId = model.CategoryId;

            _context.SaveChanges();
        }

        public void DeleteProduct(int id)
        {
            var product = _context.Products.Find(id);

            if (product == null)
                throw new Exception("Product not found.");

            _context.Products.Remove(product);
            _context.SaveChanges();
        }

        public Product GetProductById(int id)
        {
            return _context.Products.FirstOrDefault(p => p.Id == id);
        }

        public void IncreaseStock(int productId,int quantity)
        {
            var product=_context.Products.Find(productId);

            if (product == null)
                throw new Exception("Product not found");
            
            int oldQty = product.Quantity;
            product.Quantity+= quantity;

            var history = new StockHistory
            {
                ProductId = productId,
                ChangeQuantity = quantity,
                OldQuantity = oldQty,
                NewQuantity = product.Quantity,
                ActionType = "IN"
            };
            _context.stockHistories.Add(history);
            _context.SaveChanges();
        }
        public void DecreaseStock(int productId, int quantity)
        {
            var product = _context.Products.Find(productId);

            if (product == null)
                throw new Exception("Product not found");

            if (product.Quantity < quantity)
                throw new Exception("Not enough stock");

            int oldQty = product.Quantity;

            product.Quantity -= quantity;

            var history = new StockHistory
            {
                ProductId = productId,
                ChangeQuantity = -quantity,
                OldQuantity = oldQty,
                NewQuantity = product.Quantity,
                ActionType = "OUT"
            };

            _context.stockHistories.Add(history);
            _context.SaveChanges();
        }
        public List<StockHistory> GetStockHistory(int productId)
        {
            return _context.stockHistories
                .Where(x => x.ProductId == productId)
                .OrderByDescending(x => x.CreatedAt)
                .ToList();
        }
    }
}