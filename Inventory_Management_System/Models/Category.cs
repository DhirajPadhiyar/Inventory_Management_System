using System.Collections.Generic;
using System.Net.Http.Headers;
using System.ComponentModel.DataAnnotations;
namespace Inventory_Management_System.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public List<Product> Products { get; set; } = new List<Product>();

    }
}
