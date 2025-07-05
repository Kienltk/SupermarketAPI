using SupermarketAPI.DTOs.Response;

namespace SupermarketAPI.Services
{
    public interface ICartService
    {
        Task<List<CartItemDTO>> GetCartItemsByUserIdAsync(int customerId);
        Task<bool> AddCartItemsAsync(int customerId, List<CartItemDTO> cartItems);
    }
}
