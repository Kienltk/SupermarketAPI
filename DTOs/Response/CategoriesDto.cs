namespace SupermarketAPI.DTOs.Response
{
    public class CategoriesDto
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public string slug { get; set; }
        public List<CategoriesDto> Children { get; set; } = new List<CategoriesDto>();
    }
}
