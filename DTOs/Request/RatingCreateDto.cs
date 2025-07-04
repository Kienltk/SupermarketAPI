namespace SupermarketAPI.DTOs.Request
{
    public class RatingCreateDto
    {
        public int ProductId { get; set; }
        public int RatingScore { get; set; }
        public string? Comment { get; set; }
    }
}
