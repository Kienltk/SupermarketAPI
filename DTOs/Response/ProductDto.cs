namespace SupermarketAPI.DTOs.Response
{
    public class ProductDto
    {
        public int ProductId { get; set; }
        public int? CategoryId { get; set; }

        public string ProductName { get; set; } = null!;

        public decimal Price { get; set; }

        public string Slug { get; set; } = null!;

        public string Status { get; set; } = null!;

        public string? Brand { get; set; }

        public string ImageUrl { get; set; } = null!;

        public int Quantity { get; set; }

        public Boolean isFavorite { get; set; }

        public double RatingScore { get; set; }

        public string? PromotionType { get; set; }

        public string? PromotionDescription { get; set; }

        public decimal? DiscountPercent { get; set; }

        public decimal? DiscountAmount { get; set; }

        public int? GiftProductId { get; set; }
        public string? GiftProductName { get; set; }
        public string? GiftProductImg { get; set; }
        public string? GiftProductSlug { get; set; }
        public decimal? GiftProductPrice { get; set; }

        public decimal? MinOrderValue { get; set; }

        public int? MinOrderQuantity { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}
