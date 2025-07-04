namespace SupermarketAPI.DTOs.Response
{
    public class RatingDto
    {
        public int RatingId { get; set; }
        public int RatingScore { get; set; }
        public string? Comment { get; set; }
        public string CustomerName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
