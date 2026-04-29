namespace Inventory_Management_System.Models
{
    public class StockHistory
    {
        public int id { get; set; }
        public int ProductId { get; set; } 
        public Product product { get; set; }
        public int ChangeQuantity { get; set; }
        public int OldQuantity { get; set; }
        public int NewQuantity { get; set; }
        public string ActionType { get; set; } // e.g., "In", "Out"
        public DateTime CreatedAt  { get; set; }= DateTime.Now;
    }
}
