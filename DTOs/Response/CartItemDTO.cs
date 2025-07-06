namespace SupermarketAPI.DTOs.Response
{
    public class CartItemDto
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; } = null!;

        public decimal Price { get; set; }

        public string Slug { get; set; } = null!;

        public string Status { get; set; } = null!;

        public string ImageUrl { get; set; } = null!;

        public int Stock { get; set; }

        public int Quantity { get; set; }

        public string? PromotionType { get; set; }

        public decimal? DiscountPercent { get; set; }

        public decimal? DiscountAmount { get; set; }

        public int? GiftProductId { get; set; }

        public decimal? MinOrderValue { get; set; }

        public int? MinOrderQuantity { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

    }
}
