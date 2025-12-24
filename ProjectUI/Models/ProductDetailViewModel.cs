using System.Collections.Generic;

namespace ProjectUI.Models
{
    public class ProductDetailViewModel
    {
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string Material { get; set; }
        public string Dimensions { get; set; }
        public string CategoryName { get; set; }
        public int StockQuantity { get; set; }
        public List<string> ImageUrls { get; set; } = new List<string>();
    }
} 