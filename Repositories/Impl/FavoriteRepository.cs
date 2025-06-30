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
                .Include(f => f.Product)
                .Where(f => f.CustomerId == customerId)
                .ToList();
        }

        public async Task<Favorite> GetFavoriteAsync(int customerId, int productId)
        {
            return await _context.Favorites.FirstOrDefaultAsync(f => f.CustomerId == customerId && f.ProductId == productId);
        }

        public async Task AddFavoriteAsync(Favorite favorite)
        {
            await _context.Favorites.AddAsync(favorite);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteFavoriteAsync(Favorite favorite)
        {
            _context.Favorites.Remove(favorite);
            await _context.SaveChangesAsync();
        }
    }
}
