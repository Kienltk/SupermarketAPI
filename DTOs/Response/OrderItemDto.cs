namespace SupermarketAPI.DTOs.Response
{
    public class OrderItemDto
    {
        public int ProductId { get; set; }

        public string? ProductName { get; set; }

        public decimal Price { get; set; }

        public string? Slug { get; set; }

        public string? ImageUrl { get; set; }

        public int Quantity { get; set; }

        public int? PromotionId { get; set; }

        public string? PromotionType { get; set; }

        public string? PromotionDescription { get; set; }

        public decimal? DiscountPercent { get; set; }

        public decimal? DiscountAmount { get; set; }

        public int? GiftProductId { get; set; }
        public string? GiftProductName { get; set; }
        public string? GiftProductImg { get; set; }
        public string? GiftProductSlug { get; set; }

        public decimal? MinOrderValue { get; set; }

        public int? MinOrderQuantity { get; set; }
    }
}
