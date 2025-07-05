using SupermarketAPI.DTOs.Response;
using SupermarketAPI.Models;

namespace SupermarketAPI.Repositories
{
    public interface ICartRepository
    {
        Task<List<CartItem>> GetCartItemsByUserIdAsync(int customerId);
        Task<Cart> GetCartByCustomerIdAsync(int customerId);
        Task CreateCartAsync(Cart cart);
        Task DeleteCartItemsAsync(List<CartItem> cartItems);
        Task AddCartItemsAsync(List<CartItem> cartItems);
    }
}
