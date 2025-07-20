using SupermarketAPI.Models;

namespace SupermarketAPI.Repositories
{
    public interface IOrderRepository
    {
        Task CreateOrder(Order order);
        Task UpdateOrder(Order order);
        Task CreateOrderDetail(OrderDetail orderDetail);
        Task<List<Order>> GetOrderByUserId(int  userId);
        Task<List<Order>> GetOrders();
        Task<List<OrderDetail>> GetOrderDetailsByOrderId(int orderId);
        Task<decimal> GetTotalIncome();
        Task<int> GetTotalOrder();
        Task<List<(Product Product, int TotalQuantity)>> GetTopProductsByCategoryIdAsync(int categoryId, int limit);
        Task<List<(DateTime Date, decimal Total)>> GetRevenueByDateAsync();

    }
}
