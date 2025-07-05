using Microsoft.EntityFrameworkCore;
using SupermarketAPI.Data;
using SupermarketAPI.Models;

namespace SupermarketAPI.Repositories.Impl
{
    public class CartRepository : ICartRepository
    {
        private readonly SupermarketContext _context;

        public CartRepository(SupermarketContext context)
        {
            _context = context;
        }

        public async Task<List<CartItem>> GetCartItemsByUserIdAsync(int customerId)
        {
            return await _context.CartItems
                .Include(ci => ci.Product)
                .Include(ci => ci.Product.Discounts)
                    .ThenInclude(d => d.Promotion)
                .Where(ci => ci.Cart.CustomerId == customerId)
                .ToListAsync();
        }

        public async Task<Cart> GetCartByCustomerIdAsync(int customerId)
        {
            return await _context.Carts.FirstOrDefaultAsync(c => c.CustomerId == customerId);
        }

        public async Task CreateCartAsync(Cart cart)
        {
            await _context.Carts.AddAsync(cart);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCartItemsAsync(List<CartItem> cartItems)
        {
            if (cartItems != null && cartItems.Any())
            {
                _context.CartItems.RemoveRange(cartItems);
                await _context.SaveChangesAsync();
            }
        }

        public async Task AddCartItemsAsync(List<CartItem> cartItems)
        {
            await _context.CartItems.AddRangeAsync(cartItems);
            await _context.SaveChangesAsync();
        }
    }
}
