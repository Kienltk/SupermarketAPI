namespace SupermarketAPI.DTOs.Response
{
    public class ProductDetailDto
    {
        public ProductDto ProductDto { get; set; }
        public List<ProductDto> RelatedProducts { get; set; }
    }
}
