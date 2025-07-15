using SupermarketAPI.DTOs.Response;

namespace SupermarketAPI.DTOs.Request
{
    public class OrderRequestDto
    {
        public List<OrderItemDto> Items { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentMethod { get; set; }
        public bool IsPay {  get; set; }
    }
}
