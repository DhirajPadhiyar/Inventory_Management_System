namespace Inventory_Management_System.DTOs
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        // Only what we need (no full Category object)
        public string CategoryName { get; set; }
    }
}
