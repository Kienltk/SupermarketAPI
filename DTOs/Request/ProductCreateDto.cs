using System.ComponentModel.DataAnnotations;

namespace SupermarketAPI.DTOs.Request
{
    public class ProductCreateDto
    {
        [Required(ErrorMessage = "ProductName is required")]
        [StringLength(100, ErrorMessage = "ProductName must be less than 100 characters")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Slug is required")]
        [RegularExpression(@"^[a-z0-9]+(?:-[a-z0-9]+)*$", ErrorMessage = "Slug must be lowercase, hyphen-separated")]
        public string Slug { get; set; }

        [Required(ErrorMessage = "Status is required")]
        public string Status { get; set; }

        [Required(ErrorMessage = "Quantity is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Quantity cannot be negative")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "BrandId is required")]
        public int? BrandId { get; set; }

        [Required(ErrorMessage = "ImageUrl is required")]
        //[Url(ErrorMessage = "ImageUrl must be a valid URL")]
        public string ImageUrl { get; set; }

        [Required(ErrorMessage = "UnitCost is required")]
        [Range(0.0, double.MaxValue, ErrorMessage = "UnitCost cannot be negative")]
        public decimal? UnitCost { get; set; }

        public int? PromotionId { get; set; }

        [Required(ErrorMessage = "CategoryId are required")]
        [MinLength(1, ErrorMessage = "At least one category is required")]
        public List<int>? CategoryId { get; set; }
    }
}
