using Microsoft.AspNetCore.Mvc;
using Inventory_Management_System.Models;
using Inventory_Management_System.Services;


namespace Inventory_Management_System.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]

    public class ProductsAPIController : Controller
    {
        private readonly ProductService _service;
        private readonly ILogger<ProductsAPIController> _logger;

        public ProductsAPIController(ProductService service, ILogger<ProductsAPIController> logger)
        {
            _service = service;
            _logger = logger;
        }

        //Get : API/Products 

        [HttpGet]
        public IActionResult GetAllProducts()
        {
            try
            {
                var products = _service.GetAllProducts();
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching products");

                return StatusCode(500, new
                {
                    message = "Internal Server Error"
                });
            }
        }

        // ✅ GET: api/products/5
        [HttpGet("{id}")]
        public IActionResult GetProductById(int id)
        {
            try
            {
                var product = _service.GetProductById(id);

                if (product == null)
                {
                    return NotFound(new
                    {
                        message = "Product not found"
                    });
                }
                return Ok(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching product by ID");

                return StatusCode(500, new
                {
                    message = "Internal Server Error"
                });

            }
        }

        // ✅ POST: api/products
        [HttpPost]
        public IActionResult CreateProduct([FromBody] Product product)
        {
            try
            {
                if (product == null)
                {
                    return BadRequest(new
                    {
                        Message = "Invalid product data"

                    });
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                _service.AddProduct(product);

                return Ok(new
                {
                    message = "Product Created Successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating product");
                return StatusCode(500, new
                {
                    message = "Something went wrong",
                    error = ex.Message
                });
            }
        }

        // ✅ DELETE: api/products/5
        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            try
            {
                var product = _service.GetProductById(id);

                if (product == null)
                {
                    return NotFound(new
                    {
                        message = "Product not found"
                    });
                }
                _service.DeleteProduct(id);

                return Ok(new
                {
                    message = "Product deleted successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting Product");

                return StatusCode(500, new
                {
                    message = "Something went wrong"
                });
            }
        }   
    }
}
