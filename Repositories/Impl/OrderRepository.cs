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

        public async Task<List<Order>> GetOrders()
        {
            return await _context.Orders
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Promotion)
                    .ThenInclude(p => p.GiftProduct)
                .Include(o => o.Customer)
                .ToListAsync();
        }

        public async Task<decimal> GetTotalIncome()
        {
            return await _context.Orders
                .Where(o => o.Status == "CONFIRMED")
                .SumAsync(o => o.Amount);
        }

        public async Task<int> GetTotalOrder()
        {
            return await _context.Orders.CountAsync();
        }

        public async Task UpdateOrder(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }

        public async Task<List<(Product Product, int TotalQuantity)>> GetTopProductsByCategoryIdAsync(int categoryId, int limit)
        {
            var queryResult = await _context.OrderDetails
                .Where(od => _context.ProductCategories
                    .Where(pc => pc.CategoryId == categoryId)
                    .Select(pc => pc.ProductId)
                    .Contains(od.ProductId))
                .GroupBy(od => od.Product)
                .Select(g => new
                {
                    Product = g.Key,
                    TotalQuantity = g.Sum(od => od.Quantity)
                })
                .OrderByDescending(g => g.TotalQuantity)
                .Take(limit)
                .ToListAsync();

            return queryResult.Select(g => (g.Product, g.TotalQuantity)).ToList();
        }

        public async Task<List<(DateTime Date, decimal Total)>> GetRevenueByDateAsync()
        {
            var queryResult = await _context.Orders
                .Where(o => o.Status != "CANCELLED")
                .GroupBy(o => o.DateOfPurchase.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    Total = g.Sum(o => o.Amount)
                })
                .OrderBy(g => g.Date)
                .ToListAsync();

            return queryResult.Select(g => (g.Date, g.Total)).ToList();
        }

        public async Task<List<OrderDetail>> GetOrderDetailsByOrderId(int orderId)
        {
            return await _context.OrderDetails
                .Where(od => od.OrderId == orderId)
                .ToListAsync();
        }
    }
}
