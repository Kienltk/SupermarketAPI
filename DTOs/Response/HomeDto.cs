namespace SupermarketAPI.DTOs.Response
{
    public class HomeDto
    {
        public List<ProductDto> TopRatedProducts { get; set; }
        public Dictionary<string, List<ProductDto>> ProductyByCategory { get; set; }
    }
}
