namespace SupermarketAPI.DTOs.Response
{
    public class PromotionDto
    {
        public int PromotionId { get; set; }
        public string? PromotionType { get; set; }

        public string? Description { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public decimal? DiscountPercent { get; set; }

        public decimal? DiscountAmount { get; set; }

        public int? GiftProductId { get; set; }
        public string? GiftProductName { get; set; }
        public string? GiftProductImg { get; set; }
        public string? GiftProductSlug { get; set; }

        public decimal? MinOrderValue { get; set; }

        public int? MinOrderQuantity { get; set; }
        public bool? IsActive { get; set; }
        public List<ProductDto>? Products { get; set; }
    }
}
