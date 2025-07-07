namespace SupermarketAPI.DTOs.Response
{
    public class OrderDto
    {
        public int BillId { get; set; }
        public int OrderId { get; set; }
        public string OrderStatus { get; set; }
        public DateTime DateOfPurchase { get; set; }
        public List<OrderItemDto>? OrderItems { get; set; }
        public decimal OrderAmount { get; set; }
        public decimal BillAmount { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentStatus { get; set; }
        public List<BillDetailDto> BillDetails { get; set; }
    }
}
