using Microsoft.EntityFrameworkCore;
using SupermarketAPI.Models;

namespace SupermarketAPI.Repositories
{
    public interface IFavoriteRepository
    {
        List<Favorite> GetFavoritesByCustomerIdAsync(int customerId);
        Task<Favorite> GetFavoriteAsync(int customerId, int productId);
        Task AddFavoriteAsync(Favorite favorite);
        Task DeleteFavoriteAsync(Favorite favorite);
    }
}
