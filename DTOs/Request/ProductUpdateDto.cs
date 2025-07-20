using System.ComponentModel.DataAnnotations;

namespace SupermarketAPI.DTOs.Request
{
    public class ProductUpdateDto
    {
        [StringLength(100, ErrorMessage = "ProductName must be less than 100 characters")]
        public string? ProductName { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal? Price { get; set; }

        [RegularExpression(@"^[a-z0-9]+(?:-[a-z0-9]+)*$", ErrorMessage = "Slug must be lowercase, hyphen-separated")]
        public string? Slug { get; set; }

        public string? Status { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Quantity cannot be negative")]
        public int? Quantity { get; set; }

        public int? BrandId { get; set; }

        public string? ImageUrl { get; set; }

        [Range(0.0, double.MaxValue, ErrorMessage = "UnitCost cannot be negative")]
        public decimal? UnitCost { get; set; }

        [Range(0.0, double.MaxValue, ErrorMessage = "TotalAmount cannot be negative")]
        public decimal? TotalAmount { get; set; }

        public List<int>? PromotionId { get; set; }

        public List<int>? CategoryId { get; set; }
    }
}
