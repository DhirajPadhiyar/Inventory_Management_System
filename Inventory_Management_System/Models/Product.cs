namespace Inventory_Management_System.Models
{
    public class Product
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }

        //Foreign key for Category
        public int CategoryId { get; set; }
        public Category Category { get; set; }

    }
}
