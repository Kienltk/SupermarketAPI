using SupermarketAPI.DTOs.Request;
using SupermarketAPI.DTOs.Response;

namespace SupermarketAPI.Services
{
    public interface IOrderService
    {
        Task<bool> CreateOrder(int customerId, OrderRequestDto orderRequestDto);
        Task<bool> UpdateOrder(int customerId, OrderUpdateDto orderRequestDto);
        Task<bool> UpdateBill(BillUpdateDto billUpdateDto);
        Task<List<OrderDto>> GetOrdersByCustomerId(int customerId);
        Task<List<OrderDto>> GetOrders();
    }
}
