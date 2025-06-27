using Microsoft.EntityFrameworkCore;
using SupermarketAPI.Data;
using SupermarketAPI.Models;

namespace SupermarketAPI.Repositories.Impl
{
    public class FavoriteRepository : IFavoriteRepository
    {
        private readonly SupermarketContext _context;

        public FavoriteRepository(SupermarketContext context)
        {
            _context = context;
        }

        public List<Favorite> GetFavoritesByCustomerIdAsync(int customerId)
        {
            return _context.Favorites
                .Where(f => f.CustomerId == customerId)
                .ToList();
        }
    }
}
