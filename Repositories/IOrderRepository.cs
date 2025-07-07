using SupermarketAPI.Models;

namespace SupermarketAPI.Repositories
{
    public interface IOrderRepository
    {
        Task CreateOrder(Order order);
        Task UpdateOrder(Order order);
        Task CreateOrderDetail(OrderDetail orderDetail);
        Task<List<Order>> GetOrderByUserId(int  userId);

    }
}
