namespace SupermarketAPI.DTOs.Response
{
    public class BillDetailDto
    {
        public string? ItemType { get; set; }
        public decimal? Amount { get; set; }
        public string? Description { get; set; }
    }
}
