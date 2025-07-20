using System.ComponentModel.DataAnnotations;

namespace SupermarketAPI.DTOs.Response
{
    public class BrandDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Brand name is required.")]
        public string BrandName { get; set; }
        [Required(ErrorMessage = "Slug is required.")]
        [RegularExpression(@"^[a-z0-9]+(?:-[a-z0-9]+)*$", ErrorMessage = "Slug must be lowercase, hyphen-separated")]
        public string Slug { get; set; }
    }
}
