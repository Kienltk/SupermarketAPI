namespace SupermarketAPI.DTOs.Response
{
    public class CategoriesDto
    {
        public CategoryDto CategoryDto { get; set; }
        public List<CategoriesDto> Children { get; set; } = new List<CategoriesDto>();
    }
}
