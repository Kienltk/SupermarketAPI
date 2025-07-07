using Microsoft.EntityFrameworkCore;
using SupermarketAPI.Data;
using SupermarketAPI.Models;

namespace SupermarketAPI.Repositories.Impl
{
    public class OrderRepository : IOrderRepository
    {
        private readonly SupermarketContext _context;

        public OrderRepository(SupermarketContext context)
        {
            _context = context;
        }

        public async Task CreateOrder(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
        }

        public async Task CreateOrderDetail(OrderDetail orderDetail)
        {
            _context.OrderDetails.Add(orderDetail);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Order>> GetOrderByUserId(int userId)
        {
            return await _context.Orders
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product) 
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Promotion) 
                    .ThenInclude(p => p.GiftProduct)
                .Where(o => o.CustomerId == userId)
                .ToListAsync();
        }

        public async Task UpdateOrder(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }
    }
}
