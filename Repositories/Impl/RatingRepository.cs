using Microsoft.EntityFrameworkCore;
using SupermarketAPI.Data;
using SupermarketAPI.Models;

namespace SupermarketAPI.Repositories.Impl
{
    public class RatingRepository : IRatingRepository
    {
        private readonly SupermarketContext _context;

        public RatingRepository(SupermarketContext context)
        {
            _context = context;
        }

        public double GetAvgRatingProduct(int productId)
        {
            return _context.Ratings
                .Where(r => r.ProductId == productId)
                .Average(r => (double?)r.RatingScore) ?? 0;
        }

        public async Task<List<Rating>> GetRatingsByProductIdAsync(int productId)
        {
            return await _context.Ratings
                .Include(r => r.Customer)
                .Where(r => r.ProductId == productId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<Rating?> GetRatingByIdAsync(int ratingId)
        {
            return await _context.Ratings
                .Include(r => r.Customer)
                .FirstOrDefaultAsync(r => r.RatingId == ratingId);
        }

        public async Task CreateRatingAsync(Rating rating)
        {
            _context.Ratings.Add(rating);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateRatingAsync(Rating rating)
        {
            _context.Ratings.Update(rating);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRatingAsync(int ratingId)
        {
            var rating = await _context.Ratings.FindAsync(ratingId);
            if (rating != null)
            {
                _context.Ratings.Remove(rating);
                await _context.SaveChangesAsync();
            }
        }
    }
}
