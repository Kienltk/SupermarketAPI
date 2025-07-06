using SupermarketAPI.DTOs.Response;

namespace SupermarketAPI.Services
{
    public interface ICartService
    {
        Task<List<CartItemDto>> GetCartItemsByUserIdAsync(int customerId);
        Task<bool> AddCartItemsAsync(int customerId, List<CartItemDto> cartItems);
    }
}
