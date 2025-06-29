using Microsoft.EntityFrameworkCore;
using SupermarketAPI.Data;

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
    }
}
