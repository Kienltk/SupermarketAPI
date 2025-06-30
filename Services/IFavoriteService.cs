using SupermarketAPI.DTOs.Response;
using SupermarketAPI.Repositories;

namespace SupermarketAPI.Services
{
    public interface IFavoriteService
    {
        Task<List<ProductDto>> GetFavoritesProductByUserId(int customerId);
        Task<bool> AddFavorite(int customerId, int productId);
        Task<bool> DeleteFavorite(int customerId, int productId);
    }
}
