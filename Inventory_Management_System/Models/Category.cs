using System.Collections.Generic;
using System.Net.Http.Headers;
namespace Inventory_Management_System.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Product> Products { get; set; }

    }
}
