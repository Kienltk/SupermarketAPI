namespace SupermarketAPI.DTOs.Request
{
    public class BillUpdateDto
    {
        public int BillId { get; set; }
        public string? PaymentMethod { get; set; }
        public string? PaymentStatus { get; set; }
    }
}
