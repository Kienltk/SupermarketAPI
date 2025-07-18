namespace SupermarketAPI.DTOs.Request
{
    public class ProductCreateDto
    {
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public string Slug { get; set; }
        public string Status { get; set; }
        public int Quantity { get; set; }
        public int? BrandId { get; set; }
        public string ImageUrl { get; set; }
        public decimal? UnitCost { get; set; }
        public decimal? TotalAmount { get; set; }
        public List<int>? PromotionIds { get; set; }
        public List<int>? CategoryIds { get; set; }
    }
}
