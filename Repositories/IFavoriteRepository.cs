using Microsoft.EntityFrameworkCore;
using SupermarketAPI.Models;

namespace SupermarketAPI.Repositories
{
    public interface IFavoriteRepository
    {
        List<Favorite> GetFavoritesByCustomerIdAsync(int customerId);
    }
}
